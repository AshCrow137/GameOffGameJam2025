using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity, IMadnessable
{
    private Resource currentResources = new Resource();
    public List<HexTile> occupiedTiles { get; protected set; } = new();
    [SerializeField]
    protected List<BaseGridUnitScript> controlledUnits = new();
    [SerializeField]
    protected List<GridCity> controlledCities = new();

    public List<Vector3Int> visibleTiles { get; protected set; } = new();

    public Dictionary<Vector3Int, City> cities { get; protected set; } = new();
    protected bool isDefeated = false;
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

    public virtual void Initialize()
    {
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

    public bool IsTileVisible(Vector3Int position)
    {
        return visibleTiles.Contains(position);
    }

    public float madnessLevel { get; private set; } = 0f;
    [SerializeField]
    public float maxMadnessLevel { get; private set; } = 100f;

    public void IncreaseMadness(float amount)
    {
        madnessLevel += amount;
        if(madnessLevel > maxMadnessLevel)
        {
            madnessLevel = maxMadnessLevel;
        }
    }

    public void DecreaseMadness(float amount)
    {
        madnessLevel -= amount;
        if(madnessLevel < 0f)
        {
            madnessLevel = 0f;
        }
    }

    public virtual MadnessEffect GetMadnessEffects()
    {
        if(madnessLevel == MadnessCosts.PreventAttacks)
        {
            DecreaseMadness(MadnessCosts.PreventAttacks);
            return MadnessEffect.PreventAttacks;
        }
        else if(madnessLevel >= MadnessCosts.ImproveUnits)
        {
            DecreaseMadness(MadnessCosts.ImproveUnits);
            return MadnessEffect.ImproveUnits;
        }
        else if(madnessLevel >= MadnessCosts.FreeUnits)
        {
            DecreaseMadness(MadnessCosts.FreeUnits);
            return MadnessEffect.FreeUnits;
        }
        else if(madnessLevel >= MadnessCosts.BetterDeal)
        {
            DecreaseMadness(MadnessCosts.BetterDeal);
            return MadnessEffect.BetterDeal;
        }
        else
        {
            return MadnessEffect.None;
        }
    }


}