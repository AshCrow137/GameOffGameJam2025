using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(Seeker),typeof(UnitStats))]
public class BaseGridUnitScript : BaseGridEntity, IDamageable
{

    [SerializeField]
    protected bool CanMoveAfterattack = false;
    [SerializeField]
    protected GameObject movementPointsPanel;
    [SerializeField]
    protected Image movementPointsImage;
    protected List<Image> movementPointsImages = new List<Image>();
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected float MovementSpeed = 3;
    
    [SerializeField]
    protected int specialAbilityRange = 1;
    public bool aiming = false;

    [SerializeField] public AK.Wwise.Event unitAttackSoundEvent;
    [SerializeField] public AK.Wwise.Event unitLandMovementEvent;
    [SerializeField] public AK.Wwise.Event unitWaterMovementEvent;


    [SerializeField]
    protected int unitTier = 1;
    private Seeker seeker;
    private Path path;
    public int tilesRemain { get; protected set; }
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;
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

    public bool bCanExplore { get; protected set; }

    [SerializeField] private List<ResourceRequirement> resourceRequirements = new List<ResourceRequirement>();

    public int duration;
    [SerializeField]
    private Sprite abilityImage;
    [SerializeField]
    private string abilityDescription;

    private bool bStartCreatePath = false;
    private bool bWasSelected = false;
    public UnityEvent MovementFinishEvent {  get; protected set; } = new UnityEvent();
    public UnityEvent AttackFinishEvent {  get; protected set; } = new UnityEvent();
    public UnityEvent<bool> PathFinishEvent {  get; protected set; } = new UnityEvent<bool>();

    public UnitStats unitStats { get;private set; }
    private int attacksRemain;
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
    private Vector3Int previousMarkerPos = Vector3Int.one*int.MaxValue;

    public void BanExploring()
    {
        bCanExplore = false;
    }

    public List<TileState> GetPossibleSpawnTiles()
    {
        return possibleSpawnTiles;
    }

    public EntityType GetUnitType() => entityType;

    ///<summary>
    ///grid units depends on HexTilemapManager, so they should initialize after them
    ///</summary>
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        seeker = GetComponent<Seeker>();
        unitStats = GetComponent<UnitStats>();
        unitStats.Initialize();
        tilesRemain = unitStats.UnitMovementDistance.FinalMovementDistance;
        remainMovementText.text = tilesRemain.ToString();
        UpdateMovementPointsUI();
        hTM.PlaceUnitOnTile(hTM.WorldToCellPos(transform.position),this);
        movementPointsImages = new List<Image>(unitStats.UnitMovementDistance.FinalMovementDistance)
        {
            movementPointsImage
        };
        for (int i=0;i< unitStats.UnitMovementDistance.FinalMovementDistance  - 1;i++)
        {
            
                Image newImage = Instantiate(movementPointsImage, movementPointsPanel.transform);
                movementPointsImages.Add(newImage);
           
        }
        CanStandOnTiles = possibleSpawnTiles;


    }

    public override void InitializeBase(BaseKingdom owner)
    {
        base.Initialize(owner);
    }

    //Override this method to add UI message
    public override void OnEntitySelect(BaseKingdom selector)
    {
        bWasSelected = true;
        if (unitStats.UnitAttackRange.AttackRange > 1)
        {
            if (Owner is PlayerKingdom) hTM.ShowMarkersForRangeAttack(this, unitStats.UnitAttackRange.AttackRange);
            foreach (Vector3Int pos in hTM.GetCellsInRange(GetCellPosition(), unitStats.UnitAttackRange.AttackRange, EnumLibrary.AllTileStates))
            {
                hTM.PlaceColoredMarkerOnPosition(pos, MarkerColor.White);
            }
        }
        if (selector!= Owner)
        {
            UIManager.Instance.ShowMessageText($"This is not your unit!");
            return;
        }
        Debug.Log($"Select {this.name} unit");
        //GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
       
        baseSprite.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, baseSprite.color.a);

    }
   
    public override void OnEntityDeselect()
    {
        bWasSelected= false;
        hTM.RemoveAllMarkers();
        base.OnEntityDeselect();
        Debug.Log($"Deselect {this.name} unit");
        //GlobalEventManager.OnTileClickEvent.RemoveListener(OnTileClicked);
        Color ownerColor = Owner.GetKingdomColor();
        baseSprite.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, baseSprite.color.a);
        
    }
    //TODO replace Entitry with controller class and remove unit end turn listener
    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);

    }
    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (entity != Owner) return;
        

    }
    public virtual void RefreshUnit()
    {
        bWasSelected = false;
        bCanExplore = true;
        tilesRemain = unitStats.UnitMovementDistance.FinalMovementDistance;
        attacksRemain = unitStats.UnitAttacksPerTurn.FinalAttacksPerTurn;
        remainMovementText.text = tilesRemain.ToString();
        UpdateMovementPointsUI();
        bStartCreatePath = false;
    }
    //public virtual void ApplyMadnessEffect(MadnessDataStruct madnessEffect)
    //{
    //    float mod = (float)madnessEffect.CreatureStatsModifier / 100;
    //    actualMaxHealth = Health - (int)Mathf.Round( Health * mod);
    //    if(CurrentHealth>actualMaxHealth)
    //    {
    //        CurrentHealth = actualMaxHealth;
    //    }
    //    actualMeleeAttackDamage= MeleeAttackDamage -(int)Mathf.Round(MeleeAttackDamage * mod);
    //    actualRangeAttackDamage= RangeAttackDamage-(int)Mathf.Round(RangeAttackDamage * mod);
    //    actualRetallitionAttackDamage = RetallitionAttackDamage - (int)Mathf.Round(RetallitionAttackDamage * mod);

    //}
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
    public virtual void  OnTileClicked(Vector3Int cellPos)
    {
        BaseGridUnitScript targetedUnit = hTM.GetUnitOnTile(cellPos);
        GridCity city = hTM.GetCityOnTile(cellPos);
        if( city != null)
        {
            TryToAttack(city, cellPos);
            
        }
        else if(targetedUnit != null)
        {
            TryToAttack(targetedUnit, cellPos);
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
   public bool TryToAttack(BaseGridEntity targetUnit, Vector3Int targetUnitPosition)
    {
        if (!targetUnit)
        {
            AttackFinishEvent.Invoke();
            return false;
        }
        if (targetUnit.GetOwner() == Owner)
        {
            UIManager.Instance.ShowMessageText($"You try to attack your kingdom unit!");
            AttackFinishEvent.Invoke();
            return false;
        }
        bTryToAttack = true;
        attackTarget = targetUnit;
        int intDistance = hTM.GetDistanceInCells(UnitPositionOnCell(), targetUnitPosition);
        Debug.Log($"Distance between {this.name} and target cell: {intDistance}");
        if(intDistance <=unitStats.UnitAttackRange.FinalAttackRange) 
        {
            Debug.Log($"{name} try to attack {targetUnit.name}!");
            if(attacksRemain>0)
            {
                attacksRemain--;
                Attack(targetUnit);
                return true;
            }
            else
            {
                UIManager.Instance.ShowMessageText($"This unit has not enough attacks!");
                AttackFinishEvent.Invoke();
                bTryToAttack = false;
                return false;
            }
            
        }
        //Calculate direction of attack (depends on mouse position on target unit's cell) and move unit to calculated cell
        else
        {
            if(unitStats.UnitAttackRange.FinalAttackRange==1)
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
                    if(TryToMoveUnitToTile(resPos))
                    {
                        AttackFinishEvent.Invoke();
                        return true;
                    }
                    else
                    {
                        AttackFinishEvent.Invoke();
                        return false;
                    }
                }

            }
            if(Owner is PlayerKingdom) UIManager.Instance.ShowMessageText($"Target unit too far!");
            AttackFinishEvent.Invoke();
            return false;
        }
        
    }

    private void OnMouseEnter()
    {
        // added InputManager.instance.selectedUnit != null cause could be city selection
        if (UIUtility.bHasSelectedEntity && UIUtility.selectedUnit != null && UIUtility.selectedUnit != this && UIUtility.selectedUnit.GetOwner() != Owner && UIUtility.selectedUnit.unitStats.UnitAttackRange.FinalAttackRange == 1)
        {
            InputManager.instance.SetAttackCursor();
        }
    }
    //show marker tile if we try to attack this unit
    void OnMouseOver()
    {
        
        if (UIUtility.bHasSelectedEntity&& UIUtility.selectedUnit != null&&UIUtility.selectedUnit!=this&& UIUtility.selectedUnit.GetOwner()!=Owner&& UIUtility.selectedUnit.unitStats.UnitAttackRange.FinalAttackRange==1)
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
        previousMarkerPos = Vector3Int.one * int.MaxValue;
        InputManager.instance.SetDefaultCursor();
    }

    /// <summary>
    /// Base attack function, controls what kind of attack is used, ranged or melee
    /// </summary>
    /// <param name="targetEntity"></param>
    protected virtual void Attack(BaseGridEntity targetEntity)
    {
        animator.Play("Attack", 0, 0);
        if (hTM.GetDistanceInCells(hTM.WorldToCellPos(transform.position), hTM.WorldToCellPos(targetEntity.transform.position)) == 1)
        {
            targetEntity.TakeDamage(unitStats.UnitMeleeDamage.FinalMeleeDamage, this, false);
            
        }
        else
        {
            targetEntity.TakeDamage(unitStats.UnitRangedDamage.FinalRangedDamage, this, false);
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
        unitAttackSoundEvent.Post(gameObject);
        UIManager.Instance.UpdateLife(this);
        hTM.RemoveAllMarkers();
        AttackFinishEvent.Invoke();
    }

    /// <summary>
    /// calculate and apply final damage to unit
    /// </summary>
    /// <param name="amount">amount of taken damage before calculation</param>
    /// <param name="attacker">unit that attack this unit</param>
    /// <param name="retallitionAttack">bool if this damage was retallition damage or not</param>
    public override void TakeDamage(int amount,BaseGridUnitScript attacker,bool retallitionAttack)
    {
        animator.Play("TakeDamage", 0, 0);
        int resultDamage = amount;
        if(!retallitionAttack)
        {
            //if not retallition  damage calculate result damage using matrix of units
            resultDamage = (int)Mathf.Round( resultDamage * GetDamageModifier(attacker.entityType, entityType));
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
            attacker.TakeDamage(unitStats.UnitCounterAttack.FinalCounterattack, this, true);
            unitAttackSoundEvent.Post(gameObject);
        }
        
    }

    /// <summary>
    /// invokes when unit dies
    /// </summary>
    public override void Death()
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
        base.Death(); // Remove from entity directory
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
    public bool TryToMoveUnitToTile( Vector3Int cellPos)
    {
        if (tilesRemain > 0 && !bIsMoving&&!bStartCreatePath)
        {

            bStartCreatePath = true;
            startingPosition = GetCellPosition();
            hTM.RemoveUnitFromTile(hTM.PositionToCellPosition(transform.position));
            hTM.SetTileState(hTM.PositionToCellPosition(transform.position), TileState.Default);
            CreatePath(hTM.CellToWorldPos(cellPos));
            return true;

        }
        else
        {
            if(Owner is PlayerKingdom) UIManager.Instance.ShowMessageText($"Not enough movement points remain!");
            MovementFinishEvent.Invoke();
            return false;
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
                if(tilesRemain>=path.vectorPath.Count)
                {
                    hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(path.vectorPath[path.vectorPath.Count - 1]), this);
                }
                else
                {
                    hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(path.vectorPath[tilesRemain]), this);
                }
                PathFinishEvent.Invoke(true);
                // Store initial cell position for vision updates
                previousCellPosition = GetCellPosition();
            }
            else
            {
                //
                PathFinishEvent.Invoke(false);
                hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position), this);
                UIManager.Instance.ShowMessageText($"Unavailable tile for {this.gameObject.name} unit!");
            }
            
            
        }
        else
        {
            PathFinishEvent.Invoke(false);
            path = null;
            Debug.LogError(p.errorLog.ToString());
            hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position), this);
        }
        bStartCreatePath = false;
    }
    //Main movement cycle
    private void  MovementCycle()
    {
        if (bIsMoving)
        {
            animator.SetBool("isMoving", true);
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
                        if(Owner is PlayerKingdom||PlayerKingdom.Instance.GetComponent<VisionManager>().CanSee(entityVision))
                        {
                            TileState tileUnderUnit = hTM.GetTileState(hTM.WorldToCellPos(path.vectorPath[CurrentWaypoint]));
                            switch (tileUnderUnit)
                            {
                                case TileState.Land:
                                    unitLandMovementEvent.Post(gameObject);
                                    break;
                                case TileState.Water:
                                    unitWaterMovementEvent.Post(gameObject);
                                    break;
                            }
                        }
                            
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
                        animator.SetBool("isMoving", false);
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
                animator.SetBool("isMoving", false);
            }
            MoveTo(path.vectorPath[CurrentWaypoint]);
        }
    }
    /// <summary>
    /// invokes when unit finnish his movement
    /// </summary>
    private void OnEndOfPathReached()
    {
        TreasureChest chest = hTM.GetTreasureChestOnTile(GetCellPosition());
        if (chest != null)
        {
            chest.Collect(Owner);
        }
        hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position),this);
        bIsMoving = false;
        MovementFinishEvent.Invoke();
        // Final vision update at destination
        Vector3Int finalCellPosition = GetCellPosition();
        transform.position = hTM.CellToWorldPos(finalCellPosition);
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
        if (unitStats.UnitAttackRange.FinalAttackRange > 1&&bWasSelected)
        {
            hTM.RemoveAllMarkers();
            if (Owner is PlayerKingdom) hTM.ShowMarkersForRangeAttack(this, unitStats.UnitAttackRange.FinalAttackRange);
            foreach (Vector3Int pos in hTM.GetCellsInRange(GetCellPosition(), unitStats.UnitAttackRange.FinalAttackRange, EnumLibrary.AllTileStates))
            {
                hTM.PlaceColoredMarkerOnPosition(pos, MarkerColor.White);
            }
        }
    }
    private void UpdateMovementPointsUI()
    {
        if(tilesRemain< unitStats.UnitMovementDistance.FinalMovementDistance)
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
    public void DecreaseSpeedForFirstTurn()
    {
        tilesRemain--;
        UpdateMovementPointsUI();
    }
    public int GetFinalDamageWithModifiers(BaseGridEntity attacker, BaseGridEntity defender)
    {
        if (unitStats.UnitAttackRange.FinalAttackRange > 1)
        {
            return (int)Mathf.Round(unitStats.UnitRangedDamage.FinalRangedDamage * GetDamageModifier(attacker.entityType, entityType));
        }
        else return (int)Mathf.Round(unitStats.UnitMeleeDamage.FinalMeleeDamage * GetDamageModifier(attacker.entityType, entityType)); ;
    }
    //public virtual void SpecialAbility() { }
    public virtual void SpecialAbility() { animator.Play("Attack", 0, 0); }
    public virtual void OnChosingTile() { }

    protected virtual void Update()
    {
        MovementCycle();
    }

    
  
  








    public Sprite GetAbilityImage()
    {
        return abilityImage;
    }
    public string GetAbilityDescription() 
    {
        return abilityDescription;
    }

    public int GetRemainAttacksCount()
    {
        return attacksRemain;
    }
    public List<TileState> GetWalkableTiles()
    {
        //TODO combine spawn tiles with seeker's walkable tiles
        return possibleSpawnTiles;
    }
    public int GetTier()
    {
        return unitTier;
    }
}
