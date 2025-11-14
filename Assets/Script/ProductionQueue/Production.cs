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
    }

    public void stratProduction(){
        isStarted = true;
    }

    // public void endProduction(){
    //     isStarted = false;
    //     turnsRemaining = 0;
    // }

    public void updateProduction(){
        turnsRemaining--;
    }
}
