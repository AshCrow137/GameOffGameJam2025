using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity, IMadnessable
{
    private Resource currentResources = new Resource();
    public List<HexTile> occupiedTiles { get; protected set; } = new();
    [SerializeField]
    protected List<BaseGridUnitScript> controlledUnits = new();

    public List<HexTile> visibleTiles { get; protected set; } = new();

    public Dictionary<Vector3Int, City> cities cities { get; protected set; } = new();
    public void AddCity(City city)
    {
        cities.Add(city.position, city);
    }
    
    public Dictionary<AIKingdom, int> relationsWithOtherKingdoms { get; protected set; } = new();
    [SerializeField]
    protected Color kingdomColor  = new Color();

    public Color GetKingdomColor() { return kingdomColor; }

    public void Initialize()
    {
        // Initializing controlled units
        foreach ( BaseGridUnitScript unit in controlledUnits)
        {
            unit.Initialize(this);
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