using UnityEngine;
using System.Collections.Generic;
using System;

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
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
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

    private void OnStartTurn(BaseKingdom kingdom)
    {
        if (kingdom != this) return;
    }

    private void OnEndTurn(BaseKingdom kingdom)
    {
        if (kingdom != this) return;

        //TODO: fazer um for para cada unidade para saber quantas estão a 5 de distancia das cidades
        // usando os metodos get cell position e get distance is cells
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

    public int madnessLevel { get; private set; } = 0;
    [SerializeField]
    public int maxMadnessLevel { get; private set; } = 100;

    public void IncreaseMadness(int amount)
    {
        madnessLevel += amount;
        if(madnessLevel > maxMadnessLevel)
        {
            madnessLevel = maxMadnessLevel;
        }
    }

    public void DecreaseMadness(int amount)
    {
        madnessLevel -= amount;
        if(madnessLevel < 0)
        {
            madnessLevel = 0;
        }
    }

    public virtual MadnessEffect GetMadnessEffects()
    {
        if(madnessLevel == MadnessCosts.almostDefeat)
        {
            return MadnessEffect.almostDefeat;
        }
        else if(madnessLevel >= MadnessCosts.less35)
        {
            return MadnessEffect.less35;
        }
        else if(madnessLevel >= MadnessCosts.less25)
        {
            return MadnessEffect.less25;
        }
        else if(madnessLevel >= MadnessCosts.less10)
        {
            return MadnessEffect.less10;
        }
        else
        {
            return MadnessEffect.None;
        }
    }


}