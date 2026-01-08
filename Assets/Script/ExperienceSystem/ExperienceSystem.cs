using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem
{
    public static ExperienceSystem Instance;

    private float expGain;

    private Dictionary<UnitStats, float> Contributions;

    public void Initialize()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of ExperienceSystem detected. There should only be one instance.");
        }
    }

    public void DistributeExperience(Combat combat)
    {
        expGain = TotalExperience(combat.unitAtacked);


    }

    private float TotalExperience(UnitStats unitKilled)
    {
        return unitKilled.UnitExp.ExpModifier * (1 + 0.1f * unitKilled.UnitExp.Level);
    }

    private void CalculateContributions(Combat combat)
    {
        float killerTypeBonus = CalculateKilledTypeBonus(combat.combatDataList[0]);

        Contributions = new Dictionary<UnitStats, float>();

        
    }

    private float CalculateKilledTypeBonus(CombatData data)
    {
        if(data.UnitAtacker.entityType == EntityType.Melee)
        {
            return 20;
        }
        else
        {
            return 10;
        }
    }
}
