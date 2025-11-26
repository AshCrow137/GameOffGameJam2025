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

    public override void Death()
    {
        base.Death();
        
        // Find parent city and remove this building from it
        Vector3Int buildingPosition = GetCellPosition();
        Dictionary<Vector3Int, GridCity> allCities = CityManager.Instance.GetAllCities();
        
        foreach (var cityEntry in allCities)
        {
            GridCity city = cityEntry.Value;
            if (city.buildings.ContainsKey(buildingPosition))
            {
                city.buildings.Remove(buildingPosition);
                break;
            }
        }
        
        GetComponent<EntityVision>()?.OnDeath();
        gameObject.SetActive(false);
    }
}

