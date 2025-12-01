using UnityEngine;
using System.Collections.Generic;

public class GridBuilding : BaseGridEntity
{
    [Header("Building Data")]
    //public string buildingName;
    //public int duration;
    //public Dictionary<ResourceType, int> resources;
    public float HpForCity;

    [SerializeField]
    private Building buildingData;
    [SerializeField]
    private string BuildingFunction;

    [SerializeField]
    private Sprite buildingUISprite;

    public Sprite GetBuildingUISprite() { return buildingUISprite; }
    protected bool bIsActive = false;
    /// <summary>
    /// Initialize the GridBuilding with a Building scriptable object
    /// </summary>
    public override  void Initialize( BaseKingdom owner)
    {
        base.Initialize(owner);
        bIsActive = true;
        if (!baseSprite) baseSprite = GetComponent<SpriteRenderer>();
        Color ownerColor = Owner.GetKingdomColor();
        baseSprite.color = new Color(ownerColor.r, ownerColor.g, ownerColor.b, 0);
        // Initialize building-specific fields
        //buildingName = building.buildingName;
        ////bodySprite.GetComponent<SpriteRenderer>().sprite = building.sprite;
        //duration = building.duration;
        //resources = new Dictionary<ResourceType, int>(building.resource);

    }
    public Building GetBuilding()
    {
        return buildingData;
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


    public override void InitializeBase(BaseKingdom owner)
    {
        base.Initialize(owner);
    }
    public virtual string GetBuildingFunction()
    {
        return BuildingFunction;
    }
}

