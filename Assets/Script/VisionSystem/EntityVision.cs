using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles vision and fog of war for grid entities
/// Attached to the same GameObject as BaseGridEntity
/// </summary>
public class EntityVision : MonoBehaviour
{
    //[Header("Vision Settings")]
    //[SerializeField]
    //private int visionRadius = 1;

    [Header("Fog of War Settings")]
    [SerializeField]
    private bool visibleUnderGreyFog = false;

    private BaseGridEntity entity;
    private HexTilemapManager hTM;
    private VisionManager selfVisionManager;
    private VisionManager playerVisionManager;

    public void Initialize(BaseGridEntity gridEntity)
    {
        entity = gridEntity;
        hTM = HexTilemapManager.Instance;
        selfVisionManager = entity.GetOwner().GetComponent<VisionManager>();
        //VisionManager playerVisionManager = GlobalVisionManager.Instance.GetPlayerVisionManager();
        //if (playerVisionManager == null)
        //{
        //    Debug.LogError("Player Vision Manager not found in GlobalVisionManager!");
        //}
        UpdateFog();
        CoverByFog();
    }

    /// <summary>
    /// Called when entity dies
    /// </summary>
    public void OnDeath()
    {
        RemoveVisibility(entity.GetCellPosition());
    }

    /// <summary>
    /// Updates fog for all tiles within vision radius (includes entity's own position)
    /// </summary>
    /// <returns>List of tile positions that were updated</returns>
    public List<Vector3Int> UpdateFog()
    {
        Vector3Int entityPosition = entity.GetCellPosition();

        // Get all tiles in vision radius (includes all tile states)


        List<Vector3Int> updatedTiles = hTM.GetCellsInRange(entityPosition, entity.GetVision(), EnumLibrary.AllTileStates);

        // Update fog for each tile
        foreach (Vector3Int tilePosition in updatedTiles)
        {
            selfVisionManager.UpdateGreyFog(tilePosition);
            selfVisionManager.UpdateBlackFog(tilePosition);
            HexTilemapManager.Instance.RefreshTile(tilePosition);
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
        // Get all tiles in vision radius from old position (includes all tile states)


        List<Vector3Int> updatedTiles = hTM.GetCellsInRange(oldPosition, entity.GetVision(), EnumLibrary.AllTileStates);

        // Remove fog for each tile
        foreach (Vector3Int tilePosition in updatedTiles)
        {
            selfVisionManager.RemoveGreyFog(tilePosition);
            HexTilemapManager.Instance.RefreshTile(tilePosition);

        }

        return updatedTiles;
    }

    public void RemoveVisibility(Vector3Int oldPosition)
    {
        // Remove fog from old position
        List<Vector3Int> removedTiles = RemoveFog(oldPosition);
        VisionManager playerVisionManager = GlobalVisionManager.Instance.GetPlayerVisionManager();

        // Update visibility for removed tiles
        foreach (Vector3Int tilePosition in removedTiles)
        {
            Fog fogState = playerVisionManager.GetFogAtPosition(tilePosition);
            playerVisionManager.UpdateEntitiesAtPosition(tilePosition, fogState);
        }
    }

    /// <summary>
    /// Updates fog and then updates visibility of all entities at the affected positions
    /// </summary>
    public void UpdateVisibility()
    {
        // Update fog and get list of affected tiles
        List<Vector3Int> updatedTiles = UpdateFog();
        VisionManager playerVisionManager = GlobalVisionManager.Instance.GetPlayerVisionManager();
        // For each updated tile, get the fog state and update entities at that position
        foreach (Vector3Int tilePosition in updatedTiles)
        {
            Fog fogState = playerVisionManager.GetFogAtPosition(tilePosition);
            playerVisionManager.UpdateEntitiesAtPosition(tilePosition, fogState);
        }
    }

    /// <summary>
    /// Updates visibility when entity moves - removes fog from old position and updates fog at new position
    /// </summary>
    /// <param name="oldPosition">The previous position of the entity</param>
    public void UpdateVisibilityOnMovement(Vector3Int oldPosition)
    {
        // Remove visibility from old position
        RemoveVisibility(oldPosition);
        // Update visibility at new position
        UpdateVisibility();
        CoverByFog();
    }

    /// <summary>
    /// Gets the vision radius of this entity
    /// </summary>
    public int GetVisionRadius()
    {
        return entity.GetVision();
    }

    /// <summary>
    /// Sets the vision radius of this entity
    /// </summary>
    //public void SetVisionRadius(int radius)
    //{
    //    entity.GetVision() = radius;
    //}

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

    public void CoverByFog()
    {
        CoverByFog(GlobalVisionManager.Instance.GetPlayerVisionManager().GetFogAtPosition(entity.GetCellPosition()));
    }

    /// <summary>
    /// Covers entity with fog, hiding/showing based on fog type and entity settings
    /// </summary>
    public void CoverByFog(Fog fog)
    {
        // Player kingdom entities are never covered by fog
        if (entity.GetOwner() == GlobalVisionManager.Instance.GetPlayerVisionManager().GetComponent<BaseKingdom>())
        {
            SetEntityVisibility(true);
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

