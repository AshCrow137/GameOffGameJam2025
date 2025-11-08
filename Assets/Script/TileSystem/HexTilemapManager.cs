using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;
using static Unity.Burst.Intrinsics.X86;
using System.Linq;

/// <summary>
/// Manages hexagonal tilemap interactions, handles tile clicks and state changes
/// Stores per-tile state since Tile assets are shared ScriptableObjects
/// 
/// Useful Function: GetCellAtMousePosition() - returns the cell position under the mouse cursor
/// Useful variable: tileStates: stores the state of each tile at its grid position
/// </summary>
public class HexTilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Camera mainCamera;
    //pahtfinding variable
    [SerializeField] private Tilemap blockedTiles;

    // Dictionary to store tile states per position (since Tile assets are shared)
    private Dictionary<Vector3Int, TileState> tileStates = new Dictionary<Vector3Int, TileState>();
    private Dictionary<Vector3Int, BaseGridUnitScript> gridUnits = new Dictionary<Vector3Int, BaseGridUnitScript>();
    // Singleton instance for easy access
    public static HexTilemapManager Instance { get; private set; }

    /// <summary>
    /// Called by Bootmanager
    /// </summary>
    public void Initialize(){
        Instantiate();
        InitializeTileStates();
        tilemap.RefreshAllTiles();
        GlobalEventManager.MouseClickedEvent.AddListener(HandleTileClick);
    }

    private void Instantiate()
    {
        // Setup singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    /// <summary>
    /// Gets the cell at the mouse position
    /// </summary>
    /// <returns>The cell at the mouse position</returns>
    public Vector3Int GetCellAtMousePosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        
        // cast a 3d ray onto a 2d plane. 
        // Note: composite collider2d with outlines as geometry type does not work because that creates an edge collider with no area.
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        // Raycast to detect what was clicked (adjust layer mask if needed)

        if (!hit.collider)
        {
            // return infinite vector
            return new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        }

        Vector3 hitPoint = hit.point;
        Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);
        return cellPosition;
    }

    /// <summary>
    /// Initializes tile states for all tiles currently in the tilemap
    /// </summary>
    private void InitializeTileStates()
    {


        BoundsInt bounds = tilemap.cellBounds;
        
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile is HexTile && !tileStates.ContainsKey(pos))
            {
                // Get default state from the tile or use Land
                HexTile hexTile = tile as HexTile;
                UpdateTileWalkability(pos, hexTile.defaultState);
                //int rand = Random.Range(0, System.Enum.GetValues(typeof(TileState)).Length);
                //TileState state = (TileState)rand;
                //tileStates[pos] = state;
                //if (state==TileState.Unavailable ||  state==TileState.OccuppiedByBuilding || state== TileState.OccupiedByUnit)
                //{
                //    blockedTiles.SetTile(pos, tile);
                //    blockedTiles.RefreshTile(pos);
                //}
                
            }
        }
        


    }
    public List<Vector3Int> GetCellsInRange(Vector3Int startPos, int range, List<TileState> possibleStates = null )
    {
        possibleStates = possibleStates ?? new List<TileState> { TileState.Land, TileState.Water };
        List<Vector3Int> possibleCellsInRage = new List<Vector3Int>();
        for (int x = startPos.x-range; x<=startPos.x+range; x++)
        {
            for (int y = startPos.y - range; y <= startPos.y+range; y++)
            {
                if((y== startPos.y+range&&x == startPos.x+range)|| (y == startPos.y - range && x == startPos.x + range))
                { continue; }
                Vector3Int pos = new Vector3Int(x, y,0);
                HexTile tile = tilemap.GetTile(pos) as HexTile;
                tile.color = Color.red;
                tilemap.RefreshTile(pos);
                if (possibleStates.Contains(GetTileState(pos)))
                {
                    possibleCellsInRage.Add(pos);
                    tile.color = Color.green;
                    tilemap.RefreshTile(pos);
                }
            }
        }
        return possibleCellsInRage;
    }
    public Vector3Int PositionToCellPosition(Vector3 pos)
    {
        return tilemap.WorldToCell(pos);
        
    }
    public Color GetTileColor(TileState state)
    {
        if (state == TileState.Land)
        {
            return Color.green;
        }
        else if (state == TileState.OccuppiedByBuilding)
        {
            return Color.blue;
        }
        else if (state == TileState.OccupiedByUnit)
        {
            return Color.cyan;
        } 
        else if (state == TileState.Water)
        {
            return Color.white;
        }
        else if (state == TileState.Unavailable)
        {
            return Color.red;
        }

        return Color.green;
    }

    public int GetDistanceInCells(Vector3Int startPoint, Vector3Int endPoint)
    {
        int dx = endPoint.x - startPoint.x;     // signed deltas
        int dy = endPoint.y - startPoint.y;
        int x = Mathf.Abs(dx);  // absolute deltas
        int y = Mathf.Abs(dy);
        // special case if we start on an odd row or if we move into negative x direction
        if ((dx < 0) ^ ((startPoint.y & 1) == 1))
            x = Mathf.Max(0, x - (y + 1) / 2);
        else
            x = Mathf.Max(0, x - (y) / 2);
        return x + y;
    }
    /// <summary>
    /// Handles mouse click on the tilemap using 3D raycasting
    /// </summary>
    private void HandleTileClick(Vector3 clickedPos)
    {
        Vector3Int cellPosition = GetCellAtMousePosition();
        // if cellposition is infinite, return
        if (cellPosition.x == int.MaxValue) return;

            // Get the tile at the clicked position
        TileBase clickedTile = tilemap.GetTile(cellPosition);
        
            if (clickedTile is HexTile)
            {
            HexTile hexTile = (HexTile)clickedTile;
            
                GlobalEventManager.InvokeOnTileClickEvent(hexTile, cellPosition);
                // Get current state (default to Land if not in dictionary)
                TileState currentState = GetTileState(cellPosition);
            //Debug.Log($"clicked tile type: {currentState}");
            // Check if tile can be clicked (is available)
            if (currentState == TileState.Land)
                {
                    // Change state from Land to Occupied
                    //SetTileState(cellPosition, TileState.Occupied);
                    
                    //Debug.Log($"Tile at {cellPosition} changed from Land to Occupied");
                }
                else
                {
                    //Debug.Log($"Tile at {cellPosition} is {currentState} and cannot be clicked");
                }
            }
        
    }
    public TileState GetHoweredTileState()
    {
        Vector3Int cellPosition = GetCellAtMousePosition();
        // if cellposition is infinite, return
        if (cellPosition.x == int.MaxValue) return TileState.Unavailable;

        // Get the tile at the clicked position
        TileBase clickedTile = tilemap.GetTile(cellPosition);

        if (clickedTile is HexTile)
        {
            HexTile hexTile = (HexTile)clickedTile;
            
            return hexTile.state;
        }
        return TileState.Unavailable;
    }
    /// <summary>
    /// Manually set a tile's state (useful for initialization or programmatic changes)
    /// </summary>
    public void SetTileState(Vector3Int cellPosition, TileState newState)
    {
        TileBase tile = tilemap.GetTile(cellPosition);
        
        if (tile is HexTile)
        {
            if (newState == TileState.Default)
            {
                HexTile htile = (HexTile)tile;
                newState = htile.defaultState;
            }
            tileStates[cellPosition] = newState;
            tilemap.RefreshTile(cellPosition);
            UpdateTileWalkability(cellPosition, newState);
        }
    }
    public void PlaceUnitOnTile(Vector3Int cellPosition,BaseGridUnitScript unit)
    {
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile is HexTile)
        {
            tileStates[cellPosition] = TileState.OccupiedByUnit;
            tilemap.RefreshTile(cellPosition);
            UpdateTileWalkability(cellPosition, TileState.OccupiedByUnit);
            if(!gridUnits.TryGetValue(cellPosition, out BaseGridUnitScript unitScript))
            {
                gridUnits.Add(cellPosition, unit);
            }
            
        }
    }
    public void RemoveUnitFromTile(Vector3Int cellPosition)
    {
        if (gridUnits.TryGetValue(cellPosition, out BaseGridUnitScript unitScript))
        {
            gridUnits.Remove(cellPosition);
        }
    }
    public BaseGridUnitScript GetUnitOnTile(Vector3Int cellPosition)
    {
        if(gridUnits.TryGetValue(cellPosition,out BaseGridUnitScript unitScript))
        {
            return unitScript;
        }
        return null;
    }
    private void UpdateTileWalkability(Vector3Int cellPos,TileState state)
    {
        Bounds newBounds = new Bounds();
        newBounds.center = tilemap.CellToWorld(cellPos);
        newBounds.size = tilemap.cellSize;
        var guo = new GraphUpdateObject(newBounds);

        guo.modifyTag = true;
        guo.setTag = (int)state;
        AstarPath.active.UpdateGraphs(guo);
    }

    /// <summary>
    /// Get the state of a tile at a given cell position
    /// </summary>
    public TileState GetTileState(Vector3Int cellPosition)
    {
        // Check if state is stored in dictionary
        if (tileStates.TryGetValue(cellPosition, out TileState state))
        {
            return state;
        }
        
        // If not in dictionary, check if tile exists and get default state
        TileBase tile = tilemap.GetTile(cellPosition);
        if (tile is HexTile hexTile)
        {
            // Store default state and return it
            tileStates[cellPosition] = hexTile.defaultState;
            return hexTile.defaultState;
        }
        
        // No tile at this position
        return TileState.Unavailable;
    }

    /// <summary>
    /// Clears all tile states (useful for resetting the tilemap)
    /// </summary>
    public void ClearAllStates()
    {
        tileStates.Clear();
        InitializeTileStates();
        tilemap.RefreshAllTiles();
    }
    public Tilemap GetMainTilemap()
    {
        return tilemap;
    }
}

