using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
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
    private Tilemap tilemap;
    [SerializeField]
    private float MovementSpeed = 3;



    [SerializeField]
    private GameObject owner;
    private Seeker seeker;
    private Path path;
    private int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 0.01f;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;
    private IEnumerator movementCoroutine;

    //TODO Replace with normal UI
    [SerializeField]
    private TMP_Text remainMovementText;

    private SpriteRenderer spriteRenderer;
    public void Initialize()
    {
        
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        seeker = GetComponent<Seeker>();
        tilesRemain = MovementDistance;
        remainMovementText.text = tilesRemain.ToString();
        HexTilemapManager hTM = HexTilemapManager.Instance;
        hTM.PlaceUnitOnTile(hTM.GetMainTilemap().WorldToCell(transform.position),this);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
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
    private void OnEndTurn(GameObject entity)
    {
        if (entity ==owner) 
        {
            tilesRemain = MovementDistance;
            remainMovementText.text = tilesRemain.ToString();
        }
    }
    private void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, MovementSpeed * Time.deltaTime);
    }
    private void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {
        //if (movementCoroutine != null)
        //{
        //    return;
        //}
            if (tilesRemain>0)
        {//TODO Fix this shitcode
            
            HexTilemapManager.Instance.RemoveUnitFromTile(HexTilemapManager.Instance.GetMainTilemap().WorldToCell(transform.position));
            HexTilemapManager.Instance.SetTileState(HexTilemapManager.Instance.GetMainTilemap().WorldToCell(transform.position), TileState.Land);

            CreatePath(tilemap.CellToWorld(cellPos));
            
            movementCoroutine = MovementCycle();
            StartCoroutine(MovementCycle());
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
        }
    }
    public void MoveToTargetWithPathfinding(Vector3 pathTarget)
    {
        if (path == null)
        {
            return;
        }

        if (previousTargetPosition != pathTarget)
        {
            CreatePath(pathTarget);
        }

        MovementCycle();
        MoveTo(path.vectorPath[CurrentWaypoint]);


    }
    private IEnumerator  MovementCycle()
    {
        
        while (true)
        {
            yield return null;
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
            if(tilesRemain<0)
            {
                break;

            }
            MoveTo(path.vectorPath[CurrentWaypoint]);
        }
        OnEndOfPathReached();
    }

    private void OnEndOfPathReached()
    {
        HexTilemapManager hTM = HexTilemapManager.Instance;
        hTM.PlaceUnitOnTile(hTM.GetMainTilemap().WorldToCell(transform.position),this);
        if(movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        
    }
}
