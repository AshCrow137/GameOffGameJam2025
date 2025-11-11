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
    protected UnitType unitType;
    [SerializeField]
    protected int AttackDamage = 1;    
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
    protected bool CanMoveAfterattack = false;


    private Seeker seeker;
    private Path path;
    protected int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;
    private int attacksRemain = 1;

    //TODO Replace with normal UI
    [SerializeField]
    protected TMP_Text remainMovementText;

    private bool bIsMoving = false;
    private Vector3 PathTarget;

    [SerializeField]
    private List<TileState> possibleSpawnTiles = new List<TileState>();

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

        hTM.PlaceUnitOnTile(hTM.WorldToCellPos(transform.position),this);
        
    }
   
    
    public override void OnEntitySelect(BaseKingdom selector)
    {
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
    private void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {
        BaseGridUnitScript targetedUnit = hTM.GetUnitOnTile(cellPos);
        if(targetedUnit != null)
        {
            TryToAttack(targetedUnit, cellPos);
        }
        else
        {
            TryToMoveUnitToTile(tile, cellPos);
        }
        

        

        
    }
   private void TryToAttack(BaseGridUnitScript targetUnit, Vector3Int targetUnitPosition)
    {
        if (targetUnit.GetOwner() == Owner)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"You try to attack your kingdom unit!");
            return;
        }
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
            }
            
        }
        else
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"Target unit too far!");
        }
        
    }

    protected virtual void Attack(BaseGridUnitScript targetUnit)
    {
        targetUnit.TakeDamage(AttackDamage);
        if(!CanMoveAfterattack)
        {
            tilesRemain=0;
            remainMovementText.text = tilesRemain.ToString();
        }
    }
    public virtual void TakeDamage(int amount)
    {
        //TODO Calculate result damage
        CurrentHealth -= amount;
        HPImage.fillAmount = (float)CurrentHealth / Health;
        if (CurrentHealth <= 0 ) 
        {
            Death();
        }
    }
    protected virtual void Death()
    {
        hTM.RemoveUnitFromTile(hTM.PositionToCellPosition(transform.position));
        hTM.SetTileState(hTM.PositionToCellPosition(transform.position), TileState.Default);
        Owner.RemoveUnitFromKingdom(this);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Can be used to move unit to target position in cell, using pathfinding
    /// </summary>
    /// <param name="tile">where you wish to move</param>
    /// <param name="cellPos">tile position you wish to move </param>
    public void TryToMoveUnitToTile(HexTile tile, Vector3Int cellPos)
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

    private void OnEndOfPathReached()
    {

        hTM.PlaceUnitOnTile(hTM.PositionToCellPosition(transform.position),this);
        bIsMoving = false;

        
    }
    protected virtual void Update()
    {
       MovementCycle();
    }
  

}
