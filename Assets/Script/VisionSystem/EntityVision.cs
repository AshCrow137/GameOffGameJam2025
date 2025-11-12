using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles vision and fog of war for grid entities
/// Attached to the same GameObject as BaseGridEntity
/// </summary>
public class EntityVision : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField]
    private int visionRadius = 1;

    [Header("Fog of War Settings")]
    [SerializeField]
    private bool visibleUnderGreyFog = false;

    private BaseGridEntity entity;
    private HexTilemapManager hTM;

    public void Initialize(BaseGridEntity gridEntity)
    {
        entity = gridEntity;
        hTM = HexTilemapManager.Instance;
        UpdateFog();
    }

    /// <summary>
    /// Updates fog for all tiles within vision radius (includes entity's own position)
    /// </summary>
    /// <returns>List of tile positions that were updated</returns>
    public List<Vector3Int> UpdateFog()
    {
        List<Vector3Int> updatedTiles = new List<Vector3Int>();

        VisionManager visionManager = entity.GetOwner().GetComponent<VisionManager>();
        if(visionManager == null)
        {
            Debug.LogError("VisionManager not found on owner of entity: " + entity.name);
            return updatedTiles;
        }
        Vector3Int entityPosition = entity.GetCellPosition();

        // Loop through all tiles in vision radius (includes entity's own tile at distance 0)
        for (int x = -visionRadius; x <= visionRadius; x++)
        {
            for (int y = -visionRadius; y <= visionRadius; y++)
            {
                for (int z = -visionRadius; z <= visionRadius; z++)
                {
                    // Cube coordinate constraint
                    if (x + y + z == 0)
                    {
                        Vector3Int tilePosition = new Vector3Int(
                            entityPosition.x + x,
                            entityPosition.y + y,
                            entityPosition.z + z
                        );

                        // Verify distance
                        int distance = hTM.GetDistanceInCells(entityPosition, tilePosition);
                        if (distance <= visionRadius)
                        {
                            visionManager.UpdateGreyFog(tilePosition);
                            visionManager.UpdateBlackFog(tilePosition);
                            updatedTiles.Add(tilePosition);
                        }
                    }
                }
            }
        }

        return updatedTiles;
    }

    /// <summary>
    /// Removes fog for all tiles within vision radius from the old position (includes the old position itself)
    /// </summary>
    /// <param name="oldPosition">The old position to remove fog from</param>
    /// <returns>List of tile positions that were updated</returns>
    public List<Vector3Int> RemoveFog(Vector3Int oldPosition)
    {
        List<Vector3Int> updatedTiles = new List<Vector3Int>();

        VisionManager visionManager = entity.GetOwner().GetComponent<VisionManager>();

        // Loop through all tiles in vision radius from old position (includes old position tile at distance 0)
        for (int x = -visionRadius; x <= visionRadius; x++)
        {
            for (int y = -visionRadius; y <= visionRadius; y++)
            {
                for (int z = -visionRadius; z <= visionRadius; z++)
                {
                    // Cube coordinate constraint
                    if (x + y + z == 0)
                    {
                        Vector3Int tilePosition = new Vector3Int(
                            oldPosition.x + x,
                            oldPosition.y + y,
                            oldPosition.z + z
                        );

                        // Verify distance
                        int distance = hTM.GetDistanceInCells(oldPosition, tilePosition);
                        if (distance <= visionRadius)
                        {
                            visionManager.RemoveGreyFog(tilePosition);
                            updatedTiles.Add(tilePosition);
                        }
                    }
                }
            }
        }

        return updatedTiles;
    }

    /// <summary>
    /// Updates fog and then updates visibility of all entities at the affected positions
    /// </summary>
    public void UpdateVisibility()
    {
        // Update fog and get list of affected tiles
        List<Vector3Int> updatedTiles = UpdateFog();

        VisionManager visionManager = entity.GetOwner().GetComponent<VisionManager>();

        // For each updated tile, get the fog state and update entities at that position
        foreach (Vector3Int tilePosition in updatedTiles)
        {
            Fog fogState = visionManager.GetFogAtPosition(tilePosition);
            visionManager.UpdateEntitiesAtPosition(tilePosition, fogState);
        }
    }

    /// <summary>
    /// Updates visibility when entity moves - removes fog from old position and updates fog at new position
    /// </summary>
    /// <param name="oldPosition">The previous position of the entity</param>
    public void UpdateVisibilityOnMovement(Vector3Int oldPosition)
    {
        VisionManager visionManager = entity.GetOwner().GetComponent<VisionManager>();

        // Remove fog from old position
        List<Vector3Int> removedTiles = RemoveFog(oldPosition);

        // Update visibility for removed tiles
        foreach (Vector3Int tilePosition in removedTiles)
        {
            Fog fogState = visionManager.GetFogAtPosition(tilePosition);
            visionManager.UpdateEntitiesAtPosition(tilePosition, fogState);
        }

        // Update visibility at new position
        UpdateVisibility();
    }

    /// <summary>
    /// Gets the vision radius of this entity
    /// </summary>
    public int GetVisionRadius()
    {
        return visionRadius;
    }

    /// <summary>
    /// Sets the vision radius of this entity
    /// </summary>
    public void SetVisionRadius(int radius)
    {
        visionRadius = radius;
    }

    /// <summary>
    /// Makes the entity invisible/visible without disabling the GameObject
    /// </summary>
    public void SetEntityVisibility(bool visible)
    {
        // Hide/show all sprite renderers
        SpriteRenderer[] renderers = entity.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.enabled = visible;
        }

        // Hide/show canvas
        Canvas canvas = entity.GetRotatableCanvas();
        if (canvas != null)
        {
            canvas.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// Covers entity with fog, hiding/showing based on fog type and entity settings
    /// </summary>
    public void CoverByFog(Fog fog)
    {
        // Player kingdom entities are never covered by fog
        if (entity.GetOwner() is PlayerKingdom)
        {
            return;
        }

        bool shouldBeVisible = true;

        switch (fog)
        {
            case Fog.None:
                shouldBeVisible = true;
                break;
            case Fog.Grey:
                shouldBeVisible = visibleUnderGreyFog;
                break;
            case Fog.Black:
                shouldBeVisible = false;
                break;
        }

        SetEntityVisibility(shouldBeVisible);
    }

    /// <summary>
    /// Gets whether this entity is visible under grey fog
    /// </summary>
    public bool GetVisibleUnderGreyFog()
    {
        return visibleUnderGreyFog;
    }

    /// <summary>
    /// Sets whether this entity is visible under grey fog
    /// </summary>
    public void SetVisibleUnderGreyFog(bool visible)
    {
        visibleUnderGreyFog = visible;
    }
}

