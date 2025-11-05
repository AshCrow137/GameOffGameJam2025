using UnityEngine;
using Pathfinding;
using TMPro;

[RequireComponent(typeof(Seeker))]
public class BaseGridUnitScript : MonoBehaviour
{
    [SerializeField]
    protected int Health=2;
    [SerializeField]
    protected int CurrentHealth = 2;
    [SerializeField]
    protected int MovementDistance = 5;
    [SerializeField]
    private float MovementSpeed = 3;

    private BaseKingdom Owner;
    private Seeker seeker;
    private Path path;
    private int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;

    //TODO Replace with normal UI
    [SerializeField]
    private TMP_Text remainMovementText;

    private SpriteRenderer spriteRenderer;
    private HexTilemapManager hTM = HexTilemapManager.Instance;

    private bool bIsMoving = false;

    [SerializeField]
    private GameObject bodySprite;
    [SerializeField]
    private Vector3 rotationAngles;
    [SerializeField]
    private Transform CameraArm;
    ///<summary>
    ///grid units depends on HexTilemapManager, so they should initialize after them
    ///</summary>
    public void Initialize(BaseKingdom owner)
    {

        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        seeker = GetComponent<Seeker>();
        tilesRemain = MovementDistance;
        remainMovementText.text = tilesRemain.ToString();
        hTM = HexTilemapManager.Instance;
        hTM.PlaceUnitOnTile(hTM.GetMainTilemap().WorldToCell(transform.position),this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        Owner = owner;
    }
    public BaseKingdom GetOwner() { return Owner; }
    public void OnUnitSelect()
    {
        Debug.Log($"Select {this.name} unit");
        GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
        spriteRenderer.color = Color.gray;
    }
    public void OnUnitDeselect()
    {
        Debug.Log($"Deselect {this.name} unit");
        GlobalEventManager.OnTileClickEvent.RemoveListener(OnTileClicked);
        spriteRenderer.color = Color.white;
    }
    //TODO replace Entitry with controller class and remove unit end turn listener
    private void OnEndTurn(BaseKingdom entity)
    {
        if (entity == Owner) 
        {
            tilesRemain = MovementDistance;
            remainMovementText.text = tilesRemain.ToString();
        }
    }
    protected virtual void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed * Time.deltaTime);
    }
    private void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {

           TryToMoveUnitToTile(tile, cellPos);

        
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

            CreatePath(hTM.GetMainTilemap().CellToWorld(cellPos));


        }
    }
    private void CreatePath(Vector3 target)
    {
        seeker.StartPath(transform.position, target, OnPathComplete);
    }
    
    private void OnPathComplete(Path p)
    {
        // Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            CurrentWaypoint = 0;
            bIsMoving = true;
        }
        else
        {
            path = null;
            Debug.LogError(p.errorLog.ToString());
        }
    }
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
                bIsMoving = false;
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
  
    void LateUpdate()
    {
        //rotating unit body sprite
        bodySprite.transform.localRotation = Quaternion.Euler(new Vector3(CameraArm.transform.rotation.eulerAngles.z+90,-90,-90));    
    }
}
