using UnityEngine;

public class Production
{
    public Vector3Int position;
    public int turnsRemaining;

    public bool isStarted;
    public ProductionType productionType;
    public Building building;

    public Production(Vector3Int position, ProductionType productionType, int duration, Building building=null)
    {
        this.building = building;
        this.position = position;
        this.turnsRemaining = duration;
        this.productionType = productionType;
        this.isStarted = false;
    }

    public bool StartProduction(GridCity city){
        if(isStarted)
        {
            Debug.Log("Continuing production of " + productionType + " at " + position);
        }
        if(productionType == ProductionType.Building && BuildingManager.Instance.CheckAndStartConstruction(city, building, position))
        {
            isStarted = true;
        }
        return isStarted;
    }

    public void EndProduction(){
        if(productionType == ProductionType.Building){
            BuildingManager.Instance.PlaceBuilding(building, position);
        }
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
        return false;
    }
}
