using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Seeker))]
public class BaseGridUnitScript : BaseGridEntity
{
    [Header("Unit stats")]
    [SerializeField]
    public UnitType unitType;
    [SerializeField]
    protected int MeleeAttackDamage = 1;
    [SerializeField]
    protected int RangeAttackDamage = 1;
    [SerializeField]
    protected int RetallitionAttackDamage = 1;
    [SerializeField]
    protected int AttackRange = 1;
    [SerializeField]
    protected int MovementDistance = 5;
    //TODO Convert cost to sctuct and add cost variable;
    [SerializeField]
    protected int ProductionTime = 1;
    [SerializeField]
    private float MovementSpeed = 3;
    [SerializeField]
    protected int AttacksPerTurn = 1;
    [SerializeField]
    protected bool CanMoveAfterattack = false;
    [SerializeField]
    protected GameObject movementPointsPanel;
    [SerializeField]
    protected Image movementPointsImage;
    protected List<Image> movementPointsImages = new List<Image>();
    
    [SerializeField]
    protected int specialAbilityRange = 1;
    public bool aiming = false;



    private Seeker seeker;
    private Path path;
    protected int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;
    private int attacksRemain = 1;
    protected Vector3Int startingPosition;
    protected float distanceTravelled = 0;

    //TODO Replace with normal UI
    [SerializeField]
    protected TMP_Text remainMovementText;

    private bool bIsMoving = false;
    private Vector3 PathTarget;

    [SerializeField]
    private List<TileState> possibleSpawnTiles = new List<TileState>();

    private bool bTryToAttack = false;
    private BaseGridEntity attackTarget;

    // Vision system - track previous position for fog updates during movement
    private Vector3Int previousCellPosition;


    [SerializeField] private List<ResourceRequirement> resourceRequirements = new List<ResourceRequirement>();

    public int duration;
    [SerializeField]
    private Sprite abilityImage;
    [SerializeField]
    private string abilityDescription;

    protected int actualMaxHealth;
    protected int actualMeleeAttackDamage = 1;
    protected int actualRangeAttackDamage = 1;
    protected int actualRetallitionAttackDamage = 1;

    public Dictionary<ResourceType, int> resource
    {
        get
        {
            Dictionary<ResourceType, int> dict = new Dictionary<ResourceType, int>();
            foreach (var req in resourceRequirements)
            {
                dict[req.resourceType] = req.amount;
            }
            return dict;
        }
    }

    private static readonly float[,] AttackModifiers = {
    /*Cavalry*/ {1.0f,1.0f,1.5f,1f },
    /*Infantry*/{1.5f,1.0f,1.0f,1f },
    /*Archers*/ {1.0f,1.5f,1.0f,1f },
    /*Special*/ {1.0f,1.0f,1.0f,1f },
    };
    private float GetDamageModifier(UnitType attacker,UnitType defender)
    {
        return AttackModifiers[(int)attacker, (int)defender];
    }
    public List<TileState> GetPossibleSpawnTiles()
    {
        return possibleSpawnTiles;
    }

    public UnitType GetUnitType() => unitType;

    ///<summary>
    ///grid units depends on HexTilemapManager, so they should initialize after them
    ///</summary>
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        seeker = GetComponent<Seeker>();
        tilesRemain = MovementDistance;
        remainMovementText.text = tilesRemain.ToString();
        UpdateMovementPointsUI();
        hTM.PlaceUnitOnTile(hTM.WorldToCellPos(transform.position),this);
        movementPointsImages = new List<Image>(MovementDistance)
        {
            movementPointsImage
        };
        for (int i=0;i<MovementDistance-1;i++)
        {
            
                Image newImage = Instantiate(movementPointsImage, movementPointsPanel.transform);
                movementPointsImages.Add(newImage);
           
        }
        actualMaxHealth = Health;
        actualMeleeAttackDamage = MeleeAttackDamage;
        actualRangeAttackDamage = RangeAttackDamage;
        actualRetallitionAttackDamage = RetallitionAttackDamage;
    }
    //Override this method to add UI message
    public override void OnEntitySelect(BaseKingdom selector)
    {
        if (selector!= Owner)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"This is not your unit!");
            return;
        }
        Debug.Log($"Select {this.name} unit");
        GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
       
        baseSprite.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, baseSprite.color.a);
        if(AttackRange>1)
        {
            hTM.ShowMarkersForRangeAttack(this, AttackRange);
        }
        
    }
   
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        Debug.Log($"Deselect {this.name} unit");
        GlobalEventManager.OnTileClickEvent.RemoveListener(OnTileClicked);
        Color ownerColor = Owner.GetKingdomColor();
        baseSprite.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, baseSprite.color.a);
        hTM.RemoveAllMarkers();
    }
    //TODO replace Entitry with controller class and remove unit end turn listener
    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
            tilesRemain = MovementDistance;
            attacksRemain = AttacksPerTurn;
            remainMovementText.text = tilesRemain.ToString();
        UpdateMovementPointsUI();
        
    }
    public virtual void ApplyMadnessEffect(MadnessDataStruct madnessEffect)
    {
        float mod = (float)madnessEffect.CreatureStatsModifier / 100;
        actualMaxHealth = Health - (int)Mathf.Round( Health * mod);
        if(CurrentHealth>actualMaxHealth)
        {
            CurrentHealth = actualMaxHealth;
        }
        actualMeleeAttackDamage= MeleeAttackDamage -(int)Mathf.Round(MeleeAttackDamage * mod);
        actualRangeAttackDamage= RangeAttackDamage-(int)Mathf.Round(RangeAttackDamage * mod);
        actualRetallitionAttackDamage = RetallitionAttackDamage - (int)Mathf.Round(RetallitionAttackDamage * mod);

    }
    protected virtual void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed * Time.deltaTime);
    }
    public Vector3Int UnitPositionOnCell()
    {
        return hTM.WorldToCellPos(transform.position);
    }
    /// <summary>
    /// Invokes when player press alternative tile interaction button (default RightMouseButton)
    /// </summary>
    /// <param name="tile">clicked tile</param>
    /// <param name="cellPos">position of clicked tile</param>
    protected void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {
        BaseGridUnitScript targetedUnit = hTM.GetUnitOnTile(cellPos);
        GridCity city = hTM.GetCityOnTile(cellPos);
        if(targetedUnit != null)
        {
            TryToAttack(targetedUnit, cellPos);
            
        }
        else if(city!=null)
        {
            TryToAttack(city, cellPos);
        }
        else
        {
            TryToMoveUnitToTile( cellPos);
        }
        

        

        
    }
    /// <summary>
    /// determinate can we attack unit or not
    /// </summary>
    /// <param name="targetUnit">attacked unit</param>
    /// <param name="targetUnitPosition">attacked unit position</param>
   private void TryToAttack(BaseGridEntity targetUnit, Vector3Int targetUnitPosition)
    {
        if (targetUnit.GetOwner() == Owner)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"You try to attack your kingdom unit!");
            return;
        }
        bTryToAttack = true;
        attackTarget = targetUnit;
        int intDistance = hTM.GetDistanceInCells(UnitPositionOnCell(), targetUnitPosition);
        Debug.Log($"Distance between {this.name} and target cell: {intDistance}");
        if(intDistance <=AttackRange) 
        {
            Debug.Log($"{name} try to attack {targetUnit.name}!");
            if(attacksRemain>0)
            {
                attacksRemain--;
                Attack(targetUnit);
            }
            else
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"This unit has not enough attacks!");
                bTryToAttack = false;
            }
            
        }
        //Calculate direction of attack (depends on mouse position on target unit's cell) and move unit to calculated cell
        else
        {
            if(AttackRange==1)
            {
                var tilePos = hTM.CellToWorldPos(targetUnitPosition);
                Vector3 mousePos = InputManager.instance.GetWorldPositionOnMousePosition();
                float angle = Mathf.Atan2(mousePos.y - tilePos.y, mousePos.x - tilePos.x) * Mathf.Rad2Deg;

                int i = 0;
                if (angle >= 0 && angle <= 60) i=1;
                else if (angle < 0 && angle >= -60) i=0;
                else if (angle < -60 && angle >= -120) i=7;
                else if (angle < -120 && angle >= -180) i = 3;
                else if (angle <= 120 && angle > 60) i = 5;
                else if (angle < 180 && angle > 120) i = 2;
                var node = AstarPath.active.data.gridGraph.GetNearest(tilePos).node as GridNode;
                var nodeConnections = AstarPath.active.data.gridGraph.GetNodeConnection(node, i);
                var nodePos = (Vector3)nodeConnections.position;
                Vector3Int resPos = hTM.WorldToCellPos(nodePos);
                //Debug.Log($"selected angle: {angle}");
                int distance = hTM.GetDistanceInCells(GetCellPosition(), resPos);
                if (distance <= tilesRemain)
                {
                    TryToMoveUnitToTile(resPos);
                }

            }
            GlobalEventManager.InvokeShowUIMessageEvent($"Target unit too far!");
        }
        
    }
    private Vector3Int previousMarkerPos;
    private void OnMouseEnter()
    {
        // added InputManager.instance.selectedUnit != null cause could be city selection
        if (InputManager.instance.bHasSelectedEntity && InputManager.instance.selectedUnit != null && InputManager.instance.selectedUnit != this && InputManager.instance.selectedUnit.GetOwner() != Owner && InputManager.instance.selectedUnit.AttackRange == 1)
        {
            InputManager.instance.SetAttackCursor();
        }
    }
    //show marker tile if we try to attack this unit
    void OnMouseOver()
    {
        
        if (InputManager.instance.bHasSelectedEntity&& InputManager.instance.selectedUnit != null&&InputManager.instance.selectedUnit!=this&& InputManager.instance.selectedUnit.GetOwner()!=Owner&& InputManager.instance.selectedUnit.AttackRange==1)
        {

            Vector3 mousePos = InputManager.instance.GetWorldPositionOnMousePosition();
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            int i = 0;
            if (angle >= 0 && angle <= 60) i = 1;
            else if (angle < 0 && angle >= -60) i = 0;
            else if (angle < -60 && angle >= -120) i = 7;
            else if (angle < -120 && angle >= -180) i = 3;
            else if (angle <= 120 && angle > 60) i = 5;
            else if (angle < 180 && angle > 120) i = 2;
            var node = AstarPath.active.data.gridGraph.GetNearest(transform.position).node as GridNode;
            var nodeConnections = AstarPath.active.data.gridGraph.GetNodeConnection(node, i);
            var nodePos = (Vector3)nodeConnections.position;
            Vector3Int resPos = hTM.WorldToCellPos(nodePos);
            if(resPos != previousMarkerPos)
            {
                hTM.RemoveMarkerOnTilePosition(previousMarkerPos);
                previousMarkerPos = resPos;
                hTM.PlaceMarkerOnTilePosition(resPos);
            }
        }
    }
    //resets cursor and remove markers 
    void OnMouseExit()
    {
        hTM.RemoveMarkerOnTilePosition(previousMarkerPos);
        previousMarkerPos = Vector3Int.zero;
        InputManager.instance.SetDefaultCursor();
    }

    /// <summary>
    /// Base attack function, controls what kind of attack is used, ranged or melee
    /// </summary>
    /// <param name="targetEntity"></param>
    protected virtual void Attack(BaseGridEntity targetEntity)
    {
        if (hTM.GetDistanceInCells(hTM.WorldToCellPos(transform.position), hTM.WorldToCellPos(targetEntity.transform.position)) == 1)
        {
            targetEntity.TakeDamage(actualMeleeAttackDamage, this, false);
        }
        else
        {
            targetEntity.TakeDamage(actualRangeAttackDamage, this, false);
        }
       //if property true, unit can move after attack
        if(!CanMoveAfterattack)
        {
            tilesRemain=0;
            remainMovementText.text = tilesRemain.ToString();
            UpdateMovementPointsUI();
        }
        bTryToAttack = false;
        attackTarget = null;

        UIManager.Instance.UpdateLife(this);
        hTM.RemoveAllMarkers();
    }

    /// <summary>
    /// calculate and apply final damage to unit
    /// </summary>
    /// <param name="amount">amount of taken damage before calculation</param>
    /// <param name="attacker">unit that attack this unit</param>
    /// <param name="retallitionAttack">bool if this damage was retallition damage or not</param>
    public override void TakeDamage(int amount,BaseGridUnitScript attacker,bool retallitionAttack)
    {
        int resultDamage = amount;
        if(!retallitionAttack)
        {
            //if not retallition  damage calculate result damage using matrix of units
            resultDamage = (int)Mathf.Round( resultDamage * GetDamageModifier(attacker.unitType, unitType));
        }
        CurrentHealth -= resultDamage;
        Debug.Log($"{this.gameObject.name} take {resultDamage} damage, base damage was {amount}");
        HPImage.fillAmount = (float)CurrentHealth / Health;
        if (CurrentHealth <= 0 ) 
        {
           
            
            Death();
            return;
        }
        //if attack is not retallition attack and this unit survives, this unit try to do retallition attack to it's attacker
        if(!retallitionAttack)
        {
            RetallitionAttack(attacker);
        }
    }
    /// <summary>
    /// base retallition attack
    /// </summary>
    /// <param name="attacker"></param>
    public virtual void RetallitionAttack(BaseGridUnitScript attacker)
    {
        if(hTM.GetDistanceInCells(hTM.WorldToCellPos(transform.position), hTM.WorldToCellPos(attacker.transform.position))==1)
        {
            attacker.TakeDamage(actualRetallitionAttackDamage, this, true);
        }
        
    }

    /// <summary>
    /// invokes when unit dies
    /// </summary>
    protected override void Death()
    {
        //calculate the distance to main City if equal or less of 5 increase Madness in 5 points
        if (GetDistanceToMainCity() <= 5)
        {
            Owner.IncreaseMadness(MadnessValue.EnemyUnitDead);
        }
        GetComponent<EntityVision>().OnDeath();
        hTM.RemoveUnitFromTile(hTM.PositionToCellPosition(transform.position));
        hTM.SetTileState(hTM.PositionToCellPosition(transform.position), TileState.Default);
        Owner.RemoveUnitFromKingdom(this);
        gameObject.SetActive(false);
    }

    protected int GetDistanceToMainCity()
    {
        GridCity mainCity = TurnManager.instance.GetCurrentActingKingdom().GetMainCity();
        int distanceToMainCity = HexTilemapManager.Instance.GetDistanceInCells
            (this.GetCellPosition(), mainCity.GetCellPosition());

        return distanceToMainCity;
    }


    /// <summary>
    /// Can be used to move unit to target position in cell, using pathfinding
    /// </summary>
    /// <param name="tile">where you wish to move</param>
    /// <param name="cellPos">tile position you wish to move </param>
    public void TryToMoveUnitToTile( Vector3Int cellPos)
    {
        if (tilesRemain > 0 && !bIsMoving)
        {


            startingPosition = GetCellPosition();
            hTM.RemoveUnitFromTile(hTM.PositionToCellPosition(transform.position));
            hTM.SetTileState(hTM.PositionToCellPosition(transform.position), TileState.Default);
            CreatePath(hTM.CellToWorldPos(cellPos));


        }
        else
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"Not enough movement points remain!");
        }
    }

    
    private void CreatePath(Vector3 target)
    {

        PathTarget = target;
        seeker.StartPath(transform.position, target, OnPathComplete);
    }
    /// <summary>
    /// calls when path constucting is complete
    /// </summary>
    /// <param name="p">result path</param>
    private void OnPathComplete(Path p)
    {

        if (!p.error)
        {
            //Check if path end on traversable tile, if not do not start moving.
            if( p.CanTraverse(AstarPath.active.GetNearest(PathTarget).node))
            {

                path = p;
                CurrentWaypoint = 0;
                bIsMoving = true;
                
                // Store initial cell position for vision updates
                previousCellPosition = GetCellPosition();
            }
            else
            {
                //
                
                hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position), this);
                GlobalEventManager.InvokeShowUIMessageEvent($"Unavailable tile for {this.gameObject.name} unit!");
            }
            
            
        }
        else
        {
            path = null;
            Debug.LogError(p.errorLog.ToString());
            hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position), this);
        }
    }
    //Main movement cycle
    private void  MovementCycle()
    {
        if (bIsMoving)
        {
            bReachedEndOfPath = false;
            float distanceToWaypoint;
            while (!bReachedEndOfPath)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (CurrentWaypoint + 1 < path.vectorPath.Count)
                    {
                        CurrentWaypoint++;
                        tilesRemain--;
                        remainMovementText.text = tilesRemain.ToString();
                        UpdateMovementPointsUI();
                        // Update vision when entering a new cell
                        Vector3Int currentCellPosition = GetCellPosition();
                        if (currentCellPosition != previousCellPosition)
                        {
                            EntityVision entityVision = GetComponent<EntityVision>();
                            if (entityVision != null)
                            {
                                entityVision.UpdateVisibilityOnMovement(previousCellPosition);
                                previousCellPosition = currentCellPosition;
                            }
                        }
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        bReachedEndOfPath = true;
                        OnEndOfPathReached();
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            if (tilesRemain < 0)
            {
                OnEndOfPathReached();
            }
            MoveTo(path.vectorPath[CurrentWaypoint]);
        }
    }
    /// <summary>
    /// invokes when unit finnish his movement
    /// </summary>
    private void OnEndOfPathReached()
    {

        hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position),this);
        bIsMoving = false;
        
        // Final vision update at destination
        Vector3Int finalCellPosition = GetCellPosition();
        if (finalCellPosition != previousCellPosition)
        {
            EntityVision entityVision = GetComponent<EntityVision>();
            if (entityVision != null)
            {
                entityVision.UpdateVisibilityOnMovement(previousCellPosition);
                previousCellPosition = finalCellPosition;
            }
        }
        
        if(bTryToAttack)
        {
            TryToAttack(attackTarget, attackTarget.GetCellPosition());
        }

        // Calculating distance travelled
        distanceTravelled = Vector3.Distance(startingPosition, GetCellPosition());
        startingPosition = GetCellPosition();
        if (AttackRange > 1)
        {
            hTM.ShowMarkersForRangeAttack(this, AttackRange);
        }
    }
    private void UpdateMovementPointsUI()
    {
        if(tilesRemain<MovementDistance)
        {
            int i = tilesRemain;
            if (i < 0)
            {
                i = 0;
            }
            for(int index = movementPointsImages.Count - 1; index >= i;index--)
            {
                movementPointsImages[index].color = Color.gray;
            }
            
        }
        else
        {
            foreach (var image in movementPointsImages) 
            {
                image.color = Color.white;  
            }
        }
        
    }
    public virtual void SpecialAbility() { }
    public virtual void OnChosingTile() { }

    protected virtual void Update()
    {
        MovementCycle();
    }

    public int GetCurrentHealth()
    {
        return this.CurrentHealth;
    }

    public int GetMaxHealth()
    {
        return this.actualMaxHealth;
    }
  
    public int GetMeleeDamage()
    {
        return this.actualMeleeAttackDamage;
    }

    public int GetRangeAttackDamage()
    {
        return this.actualRangeAttackDamage;
    }

    public int GetRetaliationDamage()
    {
        return this.actualRetallitionAttackDamage;
    }

    public int GetAtackDistance()
    {
        return this.AttackRange;
    }
    public Sprite GetAbilityImage()
    {
        return abilityImage;
    }
    public string GetAbilityDescription() 
    {
        return abilityDescription;
    }
}
