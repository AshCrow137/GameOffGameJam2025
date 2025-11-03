using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
/// <summary>
/// Manages hexagonal tilemap interactions, handles tile clicks and state changes
/// Stores per-tile state since Tile assets are shared ScriptableObjects
/// </summary>
public class HexTilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Camera mainCamera;
    //pahtfinding variable
    [SerializeField] private Tilemap blockedTiles;

    // Dictionary to store tile states per position (since Tile assets are shared)
    private Dictionary<Vector3Int, TileState> tileStates = new Dictionary<Vector3Int, TileState>();
    // Singleton instance for easy access
    public static HexTilemapManager Instance { get; private set; }

    /// <summary>
    /// Called by Bootmanager
    /// </summary>
    public void Initialize(){
        Instantiate();
        InitializeTileStates();
        tilemap.RefreshAllTiles();
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
                // Get default state from the tile or use Available
                HexTile hexTile = tile as HexTile;
                int rand = Random.Range(0, 3);
                TileState state = (TileState)rand;
                tileStates[pos] = state;
                if (state==TileState.Unavailable ||  state==TileState.Occupied)
                {
                    blockedTiles.SetTile(pos, tile);
                }
                
            }
        }
        print("tiles initialised");
        AstarPath.active?.Scan();
    }


    public Color GetTileColor(TileState state)
    {
        if (state == TileState.Available)
        {
            return Color.green;
        }
        else if (state == TileState.Occupied)
        {
            return Color.blue;
        }
        else if (state == TileState.Unavailable)
        {
            return Color.red;
        }

        return Color.green;
    }

    public void OnMouseClick()
    {
        
        HandleTileClick();
    }

    /// <summary>
    /// Handles mouse click on the tilemap using 3D raycasting
    /// </summary>
    private void HandleTileClick()
    {
        if (mainCamera == null || tilemap == null) return;

        // Cast a ray from camera through mouse position
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        
        // cast a 3d ray onto a 2d plane. 
        // Note: composite collider2d with outlines as geometry type does not work because that creates an edge collider with no area.
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        // Raycast to detect what was clicked (adjust layer mask if needed)
        if (hit.collider != null)
        {
            // Get the hit point in world space
            Vector3 hitPoint = hit.point;
            
            // Convert world position to tilemap cell position
            Vector3Int cellPosition = tilemap.WorldToCell(hitPoint);

    
            // Get the tile at the clicked position
            TileBase clickedTile = tilemap.GetTile(cellPosition);

            if (clickedTile is HexTile)
            {
                GlobalEventManager.InvokeOnTileClickEvent(clickedTile as HexTile,cellPosition);
                // Get current state (default to Available if not in dictionary)
                TileState currentState = GetTileState(cellPosition);
                
                // Check if tile can be clicked (is available)
                if (currentState == TileState.Available)
                {
                    // Change state from Available to Occupied
                    SetTileState(cellPosition, TileState.Occupied);
                    
                    Debug.Log($"Tile at {cellPosition} changed from Available to Occupied");
                }
                else
                {
                    Debug.Log($"Tile at {cellPosition} is {currentState} and cannot be clicked");
                }
            }
        }
    }

    /// <summary>
    /// Manually set a tile's state (useful for initialization or programmatic changes)
    /// </summary>
    public void SetTileState(Vector3Int cellPosition, TileState newState)
    {
        TileBase tile = tilemap.GetTile(cellPosition);
        
        if (tile is HexTile)
        {
            tileStates[cellPosition] = newState;
            tilemap.RefreshTile(cellPosition);
        }
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
}

