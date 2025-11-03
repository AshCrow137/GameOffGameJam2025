using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

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


    private Seeker seeker;
    private Path path;
    private int tilesRemain;
    private int CurrentWaypoint = 0;
    private float nextWaypointDistance = 3;
    private bool bReachedEndOfPath;
    private Vector3 previousTargetPosition;

    public void Initialize()
    {
        GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
        seeker = GetComponent<Seeker>();
    }

    private void OnTileClicked(HexTile tile,Vector3Int cellPos)
    {
        CreatePath(tilemap.CellToWorld(cellPos));
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
        //MoveToTarget(path.vectorPath[CurrentWaypoint]);


    }
    private void MovementCycle()
    {
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (CurrentWaypoint + 1 < path.vectorPath.Count)
                {
                    CurrentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    bReachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

    }
}
