using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity
{
    Resource currentResources = new Resource();
    List<HexTile> occupiedTiles = new();
    List<BaseGridUnitScript> controlledUnits = new();
    List<HexTile> visibleTiles = new();
    Dictionary<AIKingdom, int> relationsWithOtherKingdoms = new();
    Color kingdomColor = new Color();

    public void Initialize()
    {
        // Initializing controlled units
        foreach ( BaseGridUnitScript unit in controlledUnits)
        {
            unit.Initialize(this);
        }
    }
}