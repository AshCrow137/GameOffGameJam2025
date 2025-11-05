using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity
{
    Resource currentResources = new Resource();
    List<HexTile> occupiedTiles = new();
    //List<Unit> controlledUnits = new();
    List<HexTile> visibleTiles = new();
    Dictionary<AIKingdom, int> relationsWithOtherKingdoms = new();
}