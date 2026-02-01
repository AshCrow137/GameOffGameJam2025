using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Component that adds tiles to a kingdom's visible tiles list on click
/// </summary>
public class AddVisibleTiles : MonoBehaviour
{
    [SerializeField]
    private BaseKingdom kingdom;

    private bool isAddingTileMode = false;

    //text that displays the number of visible tiles
    [SerializeField]
    private TextMeshProUGUI visibleTilesText;

    public void Initialize()
    {
        UpdateVisibleTilesText();
        GlobalEventManager.EndTurnEvent.AddListener(EndTurn);
    }

    private void UpdateVisibleTilesText()
    {
        visibleTilesText.text = $"Visible Tiles: {kingdom.visibleTiles.Count}";
    }
    /// <summary>
    /// Start the mode to add visible tiles
    /// </summary>
    public void StartAddVisibleTileMode()
    {
        isAddingTileMode = true;
        Debug.Log("Add Visible Tile Mode: ENABLED");
    }

    /// <summary>
    /// Stop the mode to add visible tiles
    /// </summary>
    public void StopAddVisibleTileMode()
    {
        isAddingTileMode = false;
        Debug.Log("Add Visible Tile Mode: DISABLED");
    }

    public void EndTurn(BaseKingdom entity)
    {
        if (entity != kingdom)
            return;
        StopAddVisibleTileMode();
    }

    /// <summary>
    /// Onclick function that adds the current tile to visible tiles
    /// Call this from input system or UI button
    /// </summary>
    public void OnClick(CallbackContext context)
    {
        if (!context.performed) return;

        if (isAddingTileMode)
        {
            AddCurrentTileToVisibleTiles();
            UpdateVisibleTilesText();
            StopAddVisibleTileMode();

        }
    }

    /// <summary>
    /// Alternative onclick function for UI buttons (no InputAction.CallbackContext)
    /// </summary>
    public void OnClickButton()
    {
        AddCurrentTileToVisibleTiles();
    }

    /// <summary>
    /// Adds the tile at the current mouse position to the kingdom's visible tiles
    /// </summary>
    private void AddCurrentTileToVisibleTiles()
    {
        if (kingdom == null)
        {
            Debug.LogError("Kingdom reference is not set in AddVisibleTiles!");
            return;
        }

        // Get the current tile position at mouse
        Vector3Int tilePosition = HexTilemapManager.Instance.GetCellAtMousePosition();

        // Check if position is valid (not the infinite vector returned on invalid clicks)
        if (tilePosition.x == int.MaxValue)
        {
            Debug.LogWarning("No valid tile at mouse position");
            return;
        }

        // Check if tile is already visible
        if (kingdom.visibleTiles.Contains(tilePosition))
        {
            Debug.Log($"Tile at {tilePosition} is already visible for this kingdom");
            return;
        }

        // Add tile to visible tiles
        kingdom.visibleTiles.Add(tilePosition);
        Debug.Log($"Added tile at {tilePosition} to visible tiles. Total visible tiles: {kingdom.visibleTiles.Count}");
    }

    /// <summary>
    /// Manually add a specific tile position to visible tiles
    /// </summary>
    /// <param name="tilePosition">The grid position of the tile to add</param>
    public void AddTileToVisibleTiles(Vector3Int tilePosition)
    {
        if (kingdom == null)
        {
            Debug.LogError("Kingdom reference is not set in AddVisibleTiles!");
            return;
        }

        if (!kingdom.visibleTiles.Contains(tilePosition))
        {
            kingdom.visibleTiles.Add(tilePosition);
            Debug.Log($"Added tile at {tilePosition} to visible tiles");
        }
        else
        {
            Debug.Log($"Tile at {tilePosition} is already visible");
        }
    }

    /// <summary>
    /// Remove a tile from visible tiles
    /// </summary>
    /// <param name="tilePosition">The grid position of the tile to remove</param>
    public void RemoveTileFromVisibleTiles(Vector3Int tilePosition)
    {
        if (kingdom == null)
        {
            Debug.LogError("Kingdom reference is not set in AddVisibleTiles!");
            return;
        }

        if (kingdom.visibleTiles.Contains(tilePosition))
        {
            kingdom.visibleTiles.Remove(tilePosition);
            Debug.Log($"Removed tile at {tilePosition} from visible tiles");
        }
        else
        {
            Debug.Log($"Tile at {tilePosition} was not in visible tiles");
        }
    }
}

