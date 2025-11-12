using System.Collections.Generic;
using UnityEngine;

public class VisionManager : MonoBehaviour
{
    private Dictionary<Vector3Int, Fog> visionDictionary = new Dictionary<Vector3Int, Fog>();
    private Dictionary<Vector3Int, int> NotGreyFog = new Dictionary<Vector3Int, int>();
    private Dictionary<Vector3Int, bool> notBlackFog = new Dictionary<Vector3Int, bool>();
    
    public void Initialize()
    {
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
    }

    private void OnEndTurn(BaseKingdom kingdom)
    {
        ClearNotGreyFog();
    }

    /// <summary>
    /// Clears the NotGreyFog dictionary
    /// </summary>
    private void ClearNotGreyFog()
    {
        NotGreyFog.Clear();
    }

    /// <summary>
    /// Finds all entities at the given position
    /// </summary>
    public List<BaseGridEntity> FindAllEntitiesAtPosition(Vector3Int position)
    {
        List<BaseGridEntity> entities = new List<BaseGridEntity>();

        // Find unit at position
        BaseGridUnitScript unit = HexTilemapManager.Instance.GetUnitOnTile(position);
        if (unit != null)
        {
            entities.Add(unit);
        }

        // Find city at position
        GridCity city = CityManager.Instance.GetCity(position);
        if (city != null)
        {
            entities.Add(city);

            // Find building at position
            if (city.buildings.TryGetValue(position, out GridBuilding building))
            {
                entities.Add(building);
            }
        }

        return entities;
    }

    /// <summary>
    /// Updates all entities at the given position with the specified fog state
    /// </summary>
    public void UpdateEntitiesAtPosition(Vector3Int position, Fog fogState)
    {
        List<BaseGridEntity> entities = FindAllEntitiesAtPosition(position);

        foreach (BaseGridEntity entity in entities)
        {
            entity.CoverByFog(fogState);
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
}

