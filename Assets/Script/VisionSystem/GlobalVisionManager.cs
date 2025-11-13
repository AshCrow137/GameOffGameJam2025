using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global manager for vision and fog of war across the entire scene
/// how to check which entities are visible for player / bot. 
/// Assign the bot / player kingdom to variable playervisionmanager. Then click Refresh All Tiles Visibility.
/// </summary>
public class GlobalVisionManager : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField]
    private VisionManager playerVisionManager;

    public static GlobalVisionManager Instance;
    public void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Refreshes visibility for all entities in the entire scene
    /// Updates fog state and entity visibility based on current vision
    /// </summary>
    public void RefreshVisibilityForEntireScene()
    {
        if (playerVisionManager == null)
        {
            Debug.LogError("Player Vision Manager not assigned in GlobalVisionManager!");
            return;
        }

        HexTilemapManager hexTilemapManager = HexTilemapManager.Instance;
        if (hexTilemapManager == null)
        {
            Debug.LogError("HexTilemapManager instance not found!");
            return;
        }

        // Get all tile positions from the tilemap manager
        List<Vector3Int> allTilePositions = hexTilemapManager.GetAllTilePositions();

        // Refresh visibility for each tile position
        foreach (Vector3Int tilePosition in allTilePositions)
        {
            RefreshVisibilityAtPosition(tilePosition);
        }
    }

    /// <summary>
    /// Refreshes visibility at a specific position
    /// </summary>
    /// <param name="position">The position to refresh visibility at</param>
    public void RefreshVisibilityAtPosition(Vector3Int position)
    {
        if (playerVisionManager == null)
        {
            return;
        }

        // Get the current fog state at this position
        Fog fogState = playerVisionManager.GetFogAtPosition(position);

        // Update all entities at this position with the fog state
        playerVisionManager.UpdateEntitiesAtPosition(position, fogState);

        // Refresh the tile visual
        HexTilemapManager.Instance.RefreshTile(position);
    }

    /// <summary>
    /// Gets the player vision manager
    /// </summary>
    public VisionManager GetPlayerVisionManager()
    {
        return playerVisionManager;
    }

    /// <summary>
    /// Sets the player vision manager
    /// </summary>
    public void SetPlayerVisionManager(VisionManager visionManager)
    {
        playerVisionManager = visionManager;
    }
}

