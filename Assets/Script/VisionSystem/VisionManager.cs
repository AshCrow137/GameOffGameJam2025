using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// attached to a BaseKingdom gameobject.
/// CanSee method: Check if a kingdom can see a unit. do baseKingdom.GetComponent<VisionManager>().CanSee(otherEntityVision);
/// </summary>
public class VisionManager : MonoBehaviour
{
    // private Dictionary<Vector3Int, Fog> visionDictionary = new Dictionary<Vector3Int, Fog>();
    public Dictionary<Vector3Int, int> NotGreyFog { get; private set; } = new Dictionary<Vector3Int, int>();
    public Dictionary<Vector3Int, bool> notBlackFog { get; private set; } = new Dictionary<Vector3Int, bool>();


    public void Initialize()
    {
        List<Vector3Int> allTiles = HexTilemapManager.Instance.GetAllTilePositions();
        foreach (var tile in allTiles)
        {
            notBlackFog[tile] = false;
        }
    }

    /// <summary>
    /// Updates all entities at the given position with the specified fog state
    /// </summary>
    public void UpdateEntitiesAtPosition(Vector3Int position, Fog fogState)
    {
        List<BaseGridEntity> entities = HexTilemapManager.Instance.FindAllEntitiesAtPosition(position);

        foreach (BaseGridEntity entity in entities)
        {
            EntityVision entityVision = entity.GetComponent<EntityVision>();
            if (entityVision != null)
            {
                entityVision.CoverByFog(fogState);
            }
        }
    }

    /// <summary>
    /// Determines which fog state applies to a position based on the fog dictionaries
    /// </summary>
    public Fog GetFogAtPosition(Vector3Int position)
    {
        // Check if position is not in black fog
        if (notBlackFog.TryGetValue(position, out bool isNotBlack) && isNotBlack)
        {
            // Check if position is not in grey fog
            if (NotGreyFog.TryGetValue(position, out int count) && count > 0)
            {
                return Fog.None;
            }
            else
            {
                return Fog.Grey;
            }
        }
        else
        {
            return Fog.Black;
        }
    }

    /// <summary>
    /// Determines if a position is in grey fog
    /// </summary>
    public bool IsInGreyFog(Vector3Int position)
    {
        return GetFogAtPosition(position) == Fog.Grey;
    }

    /// <summary>
    /// Determines if a position is in black fog
    /// </summary>
    public bool IsInBlackFog(Vector3Int position)
    {
        return GetFogAtPosition(position) == Fog.Black;
    }

    /// <summary>
    /// Determines if a position has no fog
    /// </summary>
    public bool IsInNoFog(Vector3Int position)
    {
        return GetFogAtPosition(position) == Fog.None;
    }

    /// <summary>
    /// Updates grey fog for a tile position - increments count in NotGreyFog dictionary
    /// </summary>
    public void UpdateGreyFog(Vector3Int position)
    {
        if (NotGreyFog.TryGetValue(position, out int count))
        {
            NotGreyFog[position] = count + 1;
        }
        else
        {
            NotGreyFog[position] = 1;
        }
    }

    /// <summary>
    /// Removes grey fog for a tile position - decrements count in NotGreyFog dictionary
    /// </summary>
    public void RemoveGreyFog(Vector3Int position)
    {
        if (NotGreyFog.TryGetValue(position, out int count))
        {
            NotGreyFog[position] = count - 1;
        }
    }

    /// <summary>
    /// Updates black fog for a tile position - sets to true in notBlackFog dictionary
    /// </summary>
    public void UpdateBlackFog(Vector3Int position)
    {
        notBlackFog[position] = true;
    }


    /// <summary>
    /// Determines if an entity can be seen based on fog state at its position
    /// </summary>
    /// <param name="otherEntityVision">The EntityVision component of the entity to check</param>
    /// <returns>True if the entity can be seen, false otherwise</returns>
    public bool CanSee(EntityVision otherEntityVision)
    {
        // Get the position of the other entity
        BaseGridEntity otherEntity = otherEntityVision.GetComponent<BaseGridEntity>();
        if (otherEntity == null)
        {
            Debug.LogError("VisionEntity not attached to same GO as basegridentity. Please implement functionality to get VisionEntity position if this is desired");
            return false;
        }

        Vector3Int otherPosition = otherEntity.GetCellPosition();

        // Check the fog at that position
        Fog fogAtPosition = GetFogAtPosition(otherPosition);

        // If it's black fog, can't see
        if (fogAtPosition == Fog.Black)
        {
            return false;
        }

        // If it's grey fog, check if the entity is visible under grey fog
        if (fogAtPosition == Fog.Grey)
        {
            return otherEntityVision.GetVisibleUnderGreyFog();
        }

        // If no fog, can see
        return true;
    }
}

