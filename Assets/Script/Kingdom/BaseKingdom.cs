using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity
{
    private Resource currentResources;
    public List<HexTile> occupiedTiles { get; protected set; } = new();
    [SerializeField]
    protected List<BaseGridUnitScript> controlledUnits = new();
    [SerializeField]
    protected List<GridCity> controlledCities = new();
    [SerializeField]
    protected List<BaseGridUnitScript> unlockedUnits = new List<BaseGridUnitScript> ();
    [SerializeField]
    protected List<GridBuilding> unlockedBuildings= new List<GridBuilding> ();
    [SerializeField]
    private int StartingMagic = 50;
    [SerializeField]
    private int StartingGold = 20;
    [SerializeField]
    private int StartingMaterials = 30;
    public List<Vector3Int> visibleTiles { get; protected set; } = new();

    public Dictionary<Vector3Int, City> cities { get; protected set; } = new();
    protected bool isDefeated = false;
    protected VisionManager visionManager;
    public void AddCity(City city)
    {
        cities.Add(city.position, city);
    }
    private void DefeatCheck()
    {
        if(controlledCities.Count<=0 &&controlledUnits.Count<=0)
        {
            isDefeated=true;
            GlobalEventManager.InvokeKingdomDefeat(this);
        }
    }
    public Dictionary<AIKingdom, int> relationsWithOtherKingdoms { get; protected set; } = new();
    [SerializeField]
    protected Color kingdomColor  = new Color();

    public Color GetKingdomColor() { return kingdomColor; }
    public Resource Resources() => currentResources;
    public List<BaseGridUnitScript> ControlledUnits => controlledUnits;

    public virtual void Initialize()
    {
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
        visionManager = GetComponent<VisionManager>();
        visionManager.Initialize();
        currentResources = new Resource(this, StartingMagic,StartingGold,StartingMaterials);
        // Initializing controlled units
        foreach ( BaseGridUnitScript unit in controlledUnits)
        {
            unit.Initialize(this);
        }
        foreach ( GridCity city in controlledCities)
        {
            city.Initialize(this);
        }
    }

    protected virtual void OnStartTurn(BaseKingdom kingdom)
    {
        if (kingdom != this) return;
        foreach( BaseGridUnitScript unit in controlledUnits)
        {
            unit.RefreshUnit();
        }
    }

    protected virtual void OnEndTurn(BaseKingdom kingdom)
    {
        if (kingdom != this) return;
        int unitsCount = GetUnitsCountInRange(5);
        if (unitsCount != 0)
        {
            IncreaseMadness(unitsCount * 3);
        }
        else
        {
            DecreaseMadness(3);
        }

        
    }

    public void AddUnitToKingdom(BaseGridUnitScript unit)
    {
        if(!controlledUnits.Contains(unit))
        {
            controlledUnits.Add(unit);
        }
    }
    public void RemoveUnitFromKingdom(BaseGridUnitScript unit)
    {
        if (controlledUnits.Contains(unit))
        {
            controlledUnits.Remove(unit);
            DefeatCheck();
        }
    }
    public void AddCityToKingdom(GridCity city)
    {
        if(!controlledCities.Contains(city))
        {
            controlledCities.Add(city);
        }
    }
    public void RemoveCityFromKingdom(GridCity city)
    {
        if (controlledCities.Contains(city))
        {
            controlledCities.Remove(city);
            DefeatCheck();
        }
    }

    public GridCity GetMainCity()
    {
        return controlledCities[0];
    }

    public bool IsTileVisible(Vector3Int position)
    {
        return visibleTiles.Contains(position);
    }
    public void UnlockUnit(BaseGridUnitScript unit)
    {
        if(!unlockedUnits.Contains(unit))
        {
            unlockedUnits.Add(unit);
            CityUI.Instance.UpdateUnitButtonsInteractability();

        }
    }
    public void UnlockBuilding(GridBuilding building)
    {
        if(!unlockedBuildings.Contains(building))
        {
            unlockedBuildings.Add(building);
            CityUI.Instance.UpdateUnitButtonsInteractability();

        }
    }
    public int madnessLevel { get; private set; } = 0;
    [SerializeField]
    public int maxMadnessLevel { get; private set; } = 100;

    public int GetMadnessLevel() { return madnessLevel; }

    public virtual void IncreaseMadness(int amount)
    {
        madnessLevel += amount;
        if(madnessLevel > maxMadnessLevel)
        {
            madnessLevel = maxMadnessLevel;
        }
        Debug.Log($"Increase: Current Madness Level is: {madnessLevel}");
    }

    public virtual void DecreaseMadness(int amount)
    {
        madnessLevel -= amount;
        if(madnessLevel < 0)
        {
            madnessLevel = 0;
        }
        Debug.Log($"Decrease: Current Madness Level is: {madnessLevel}");
    }

    //public virtual MadnessEffect GetMadnessEffects()
    //{
    //    if(madnessLevel == MadnessCosts.almostDefeat)
    //    {
    //        return MadnessEffect.almostDefeat;
    //    }
    //    else if(madnessLevel >= MadnessCosts.less35)
    //    {
    //        return MadnessEffect.less35;
    //    }
    //    else if(madnessLevel >= MadnessCosts.less25)
    //    {
    //        return MadnessEffect.less25;
    //    }
    //    else if(madnessLevel >= MadnessCosts.less10)
    //    {
    //        return MadnessEffect.less10;
    //    }
    //    else
    //    {
    //        return MadnessEffect.None;
    //    }
    //}

    public List<GridCity> GetControlledCities()
    {
        return controlledCities;
    }

    public virtual int GetUnitsCountInRange(int range)
    {
        // Find units, not controlled by this kingdom in rage 
        int result = 0;
        foreach(GridCity city in controlledCities)
        {
            List<Vector3Int> tilesWithUnits = HexTilemapManager.Instance.GetCellsInRange(city.GetCellPosition(), range, new List<TileState> { TileState.OccupiedByUnit });
            foreach(Vector3Int unitPos in tilesWithUnits)
            {
                BaseGridUnitScript unit = HexTilemapManager.Instance.GetUnitOnTile(unitPos);
                if(unit!=null&&unit.GetOwner() is PlayerKingdom)
                {
                    result++;
                }
            }
        }
        //int result = 0;
        //foreach(GridCity city in controlledCities)
        //{
        //    foreach (BaseGridUnitScript unit in controlledUnits)
        //    {
        //        if(HexTilemapManager.Instance.GetDistanceInCells
        //            (city.GetCellPosition(), unit.GetCellPosition()) <= range)
        //        {
        //            result++;
        //        }
        //    }
        //}
        Debug.Log("Units in range " + range + ": " + result);
        return result;
    }
    public List<GridBuilding> GetUnlockedBuildings() { return unlockedBuildings; }
    public List<BaseGridUnitScript> GetunlockedUnits() { return unlockedUnits; }
    public VisionManager GetVisionManager() { return visionManager; }

}