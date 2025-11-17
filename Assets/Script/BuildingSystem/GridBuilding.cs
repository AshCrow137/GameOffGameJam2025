using UnityEngine;
using System.Collections.Generic;

public class GridBuilding : BaseGridEntity
{
    [Header("Building Data")]
    //public string buildingName;
    //public int duration;
    //public Dictionary<ResourceType, int> resources;
    public float HpForCity;
    

    /// <summary>
    /// Initialize the GridBuilding with a Building scriptable object
    /// </summary>
    public override  void Initialize( BaseKingdom owner)
    {
        base.Initialize(owner);

        
        // Initialize building-specific fields
        //buildingName = building.buildingName;
        ////bodySprite.GetComponent<SpriteRenderer>().sprite = building.sprite;
        //duration = building.duration;
        //resources = new Dictionary<ResourceType, int>(building.resource);
        
    }
}

