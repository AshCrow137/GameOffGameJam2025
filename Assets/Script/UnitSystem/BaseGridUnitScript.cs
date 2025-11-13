using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Collections;
using System.Linq;
using System.Data;

[RequireComponent(typeof(Seeker))]
public class BaseGridUnitScript : BaseGridEntity
{
    [Header("Unit stats")]
    [SerializeField]
    protected UnitType unitType;
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
    private int AttacksPerTurn = 1;
    [SerializeField]
    private bool CanMoveAfterattack = false;


    private Seeker seeker;
    private Path path;
    private int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;
    private int attacksRemain = 1;

    //TODO Replace with normal UI
    [SerializeField]
    private TMP_Text remainMovementText;

    private bool bIsMoving = false;
    private Vector3 PathTarget;

    [SerializeField]
    private List<TileState> possibleSpawnTiles = new List<TileState>();

    private bool bTryToAttack = false;
    private BaseGridUnitScript attackTarget;

    private static readonly float[,] AttackModifiers = {
    /*Cavalry*/{1.0f,1.0f,1.5f,1f },
    /*Infantry*/{1.5f,1.0f,1.0f,1f },
    /*Archers*/{1.0f,1.5f,1.0f,1f },
    /*Special*/{1.0f,1.0f,1.0f,1f },
    };
    private float GetDamageModifier(UnitType attacker,UnitType defender)
    {
        return AttackModifiers[(int)attacker, (int)defender];
    }
    public List<TileState> GetPossibleSpawnTiles()
    {
        return possibleSpawnTiles;
    }

    ///<summary>
    ///grid units depends on HexTilemapManager, so they should initialize after them
    ///</summary>
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        seeker = GetComponent<Seeker>();
        tilesRemain = MovementDistance;
        remainMovementText.text = tilesRemain.ToString();

        hTM.PlaceUnitOnTile(hTM.WorldToCellPos(transform.position),this);

    }
    //Override this method to add UI message
    public override void OnEntitySelect(BaseKingdom selector)
    {
        //Test UI Call
        UIManager.Instance.SelectedUnit(this);
        //
        if(selector!= Owner)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"This is not your unit!");
            return;
        }
        Debug.Log($"Select {this.name} unit");
        GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
        HPImage.color = Color.gray;
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        Debug.Log($"Deselect {this.name} unit");
        GlobalEventManager.OnTileClickEvent.RemoveListener(OnTileClicked);
        HPImage.color = Owner.GetKingdomColor();
        hTM.RemoveAllMarkers();
    }
    //TODO replace Entitry with controller class and remove unit end turn listener
    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
            tilesRemain = MovementDistance;
            attacksRemain = AttacksPerTurn;
            remainMovementText.text = tilesRemain.ToString();
        
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
    private void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {
        BaseGridUnitScript targetedUnit = hTM.GetUnitOnTile(cellPos);
        if(targetedUnit != null)
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
   private void TryToAttack(BaseGridUnitScript targetUnit, Vector3Int targetUnitPosition)
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
        if (InputManager.instance.bHasSelectedEntity && InputManager.instance.selectedUnit != this && InputManager.instance.selectedUnit.GetOwner() != Owner && InputManager.instance.selectedUnit.AttackRange == 1)
        {
            InputManager.instance.SetAttackCursor();
        }
    }
    //show marker tile if we try to attack this unit
    void OnMouseOver()
    {
        
        if (InputManager.instance.bHasSelectedEntity&&InputManager.instance.selectedUnit!=this&& InputManager.instance.selectedUnit.GetOwner()!=Owner&& InputManager.instance.selectedUnit.AttackRange==1)
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
    /// <param name="targetUnit"></param>
    protected virtual void Attack(BaseGridUnitScript targetUnit)
    {
        if (hTM.GetDistanceInCells(hTM.WorldToCellPos(transform.position), hTM.WorldToCellPos(targetUnit.transform.position)) == 1)
        {
            targetUnit.TakeDamage(MeleeAttackDamage, this, false);
        }
        else
        {
            targetUnit.TakeDamage(RangeAttackDamage, this, false);
        }
       //if property true, unit can move after attack
        if(!CanMoveAfterattack)
        {
            tilesRemain=0;
            remainMovementText.text = tilesRemain.ToString();
        }
        bTryToAttack = false;
        attackTarget = null;
    }

    /// <summary>
    /// calculate and apply final damage to unit
    /// </summary>
    /// <param name="amount">amount of taken damage before calculation</param>
    /// <param name="attacker">unit that attack this unit</param>
    /// <param name="retallitionAttack">bool if this damage was retallition damage or not</param>
    public virtual void TakeDamage(int amount,BaseGridUnitScript attacker,bool retallitionAttack)
    {
        //TODO Calculate result damage
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
            //calculate the distance to main City if equal or less of 5 increase Madness in 5 points
            if(GetDistanceToMainCity() <= 5)
            {
                TurnManager.instance.GetCurrentActingKingdom().IncreaseMadness(MadnessValue.EnemyUnitDead);
            }
            
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
            attacker.TakeDamage(RetallitionAttackDamage, this, true);
        }
        
    }

    /// <summary>
    /// invokes when unit dies
    /// </summary>
    protected virtual void Death()
    {
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
        if(bTryToAttack)
        {
            TryToAttack(attackTarget, attackTarget.GetCellPosition());
        }
        
    }
    protected virtual void Update()
    {
       MovementCycle();
    }
  

}
