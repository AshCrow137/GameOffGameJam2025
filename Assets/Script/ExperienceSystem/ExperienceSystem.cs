using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem
{
    public static ExperienceSystem Instance;

    private float expGain;

    private Dictionary<UnitStats, float> contributions;

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

        CalculateContributions(combat);

        DistributionExp(contributions);
    }

    private float TotalExperience(UnitStats unitKilled)
    {
        return unitKilled.UnitExp.ExpModifier * (1 + 0.1f * unitKilled.UnitExp.Level);
    }

    private void CalculateContributions(Combat combat)
    {
        float killerTypeBonus = CalculateKilledTypeBonus(combat.combatDataList[0]);

        contributions = new Dictionary<UnitStats, float>();

        List<CombatData> contributorsAvailable = combat.GetContributorsAvailable(combat.combatDataList);

        foreach (CombatData data in contributorsAvailable)
        {
            contributions.Add
                (
                    data.UnitAtacker, 
                        killerTypeBonus + 2 * data.DamageDealtByType[DamageType.Melee] +
                        data.DamageDealtByType[DamageType.Ranged] + data.DamageDealtByType[DamageType.Magic] +
                        data.DamageTaken + 1
                );
        }
    }

    private void DistributionExp(Dictionary<UnitStats, float> contributors)
    {
        float totalContribution = 0;
        foreach (float individualContribution in contributions.Values)
        {
            totalContribution += individualContribution;
        }

        foreach(UnitStats unit in contributors.Keys)
        {
            unit.UnitExp.AddExp(Mathf.FloorToInt(expGain * (contributions[unit] / totalContribution)));
        }
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

    public float ExpToNextLevel(UnitStats unit)
    {
        int level = unit.UnitExp.Level;
        int expMod = unit.UnitExp.ExpModifier;

        int expToNextLevel = 10 * (level + 1) * expMod / 100;
        return expToNextLevel;
    }
}
