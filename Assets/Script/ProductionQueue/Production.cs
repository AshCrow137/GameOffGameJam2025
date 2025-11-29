using UnityEngine;

public class Production
{
    public Vector3Int position;
    public int turnsRemaining;

    public bool isStarted;
    public ProductionType productionType;
    public Building building;
    public GameObject prefab;
    public GameObject placedObject;

    public Production(Vector3Int position, ProductionType productionType, int duration, Building building=null)
    {
        this.building = building;
        this.position = position;
        this.turnsRemaining = duration;
        this.productionType = productionType;
        this.isStarted = false;
    }
    public Production(Vector3Int position, ProductionType productionType, int duration, Building building = null, GameObject placedBuilding = null)
    {
        this.building = building;
        this.position = position;
        this.turnsRemaining = duration;
        this.productionType = productionType;
        this.isStarted = false;
        this.placedObject = placedBuilding;

    }
    public Production(Vector3Int position, ProductionType productionType, int duration, GameObject prefab = null)
    {
        this.prefab = prefab;
        this.position = position;
        this.turnsRemaining = duration;
        this.productionType = productionType;
        this.isStarted = false;
    }

    public Production(Vector3Int position, ProductionType productionType, int duration, GameObject prefab = null, GameObject placedUnit = null)
    {
        this.prefab = prefab;
        this.position = position;
        this.turnsRemaining = duration;
        this.productionType = productionType;
        this.isStarted = false;
        this.placedObject = placedUnit;
    }
    public bool StartProduction(GridCity city){
        if(isStarted)
        {
            Debug.Log("Continuing production of " + productionType + " at " + position);
        }
        if(productionType == ProductionType.Building)// && BuildingManager.Instance.CheckAndStartConstruction(city, building, position))
        {
            isStarted = true;
        }
        else if(productionType == ProductionType.Unit)// && UnitSpawner.Instance.CheckAndStartUnitSpawn(city, prefab, position))
        {
            isStarted = true;
        }
            return isStarted;
    }

    public void EndProduction(GridCity city){
        if(productionType == ProductionType.Building){
            BuildingManager.Instance.PlaceBuilding(building, position);
        }
        else if (productionType == ProductionType.Unit)
        {
            UnitSpawner.Instance.PlaceUnit(prefab, position, city.GetOwner());
        }
        
        // Clean up the preview/queued object from the directory before it's destroyed
        // if (placedObject != null)
        // {
        //     BaseGridEntity entity = placedObject.GetComponent<BaseGridEntity>();
        //     if (entity != null)
        //     {
        //         HexTilemapManager.Instance.RemoveEntityFromDirectory(position, entity);
        //     }
        // }
    }

    public void UpdateProduction(){
        if (isStarted)
        {
            turnsRemaining--;
        }
    }

    public bool IsComplete()
    {
        return isStarted && turnsRemaining <= 0;
    }

    public bool Cancel(GridCity city)
    {
        if (!isStarted) return true;
        if(productionType == ProductionType.Building)
        {
            BuildingManager.Instance.CancelConstruction(city, building, position);
            return true;
        }
        else
        if (productionType == ProductionType.Unit)
        {
            UnitSpawner.Instance.CancelSpawning(city, prefab, position);
            return true;
        }
        return false;
    }
}
