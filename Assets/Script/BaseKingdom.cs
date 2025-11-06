using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : Entity, IMadnessable
{
    Resource currentResources = new Resource();
    List<HexTile> occupiedTiles = new();
    List<BaseGridUnitScript> controlledUnits = new();
    List<HexTile> visibleTiles = new();
    Dictionary<AIKingdom, int> relationsWithOtherKingdoms = new();

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