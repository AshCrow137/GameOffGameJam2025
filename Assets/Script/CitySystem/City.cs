using System.Collections.Generic;
using UnityEngine;

public class City
{
    public string name;
    public Sprite sprite;
    public Vector3Int position;

    // we can add building's relation to city later
    // public List<Building> buildings;

    public float maxHP = 100f;
    public float currentHP;

    // public int visionRadius;
    public int unitSpawnRadius;

    public GameObject owner;

    // Resources this city generates per turn
    public Dictionary<ResourceType, int> resourceGainPerTurn = new Dictionary<ResourceType, int>();
    public Dictionary<Vector3Int, Building> buildings = new Dictionary<Vector3Int, Building>();

    public City(CityData cityData, Vector3Int position){
        this.name = "City Name";
        this.sprite = cityData.sprite;
        this.position = position;
        this.maxHP = cityData.maxHP;
        this.currentHP = cityData.maxHP;
        // this.visionRadius = cityData.visionRadius;
        this.unitSpawnRadius = 1;
        this.owner = null;
        this.buildings = new Dictionary<Vector3Int, Building>();
        
        // Initialize empty resource dictionary - will be populated by other means
        this.resourceGainPerTurn = new Dictionary<ResourceType, int>();
    }
}