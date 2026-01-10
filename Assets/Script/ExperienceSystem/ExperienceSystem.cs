using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
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
        Debug.Log("Total Experience to distribute calculated.");
        expGain = TotalExperience(combat.unitAtacked);
        Debug.Log("Experience to distribute: " + expGain);

        CalculateContributions(combat);

        DistributionExp(contributions);
    }

    private float TotalExperience(UnitStats unitKilled)
    {
        return unitKilled.UnitExp.ExpModifier * (1 + 0.1f * unitKilled.UnitExp.Level);
    }

    private void CalculateContributions(Combat combat)
    {
        float killerTypeBonus = 0;
        if(killerTypeBonus == 0)
        {
            killerTypeBonus = CalculateKilledTypeBonus(combat.combatDataList[0]);
        }

        contributions = new Dictionary<UnitStats, float>();

        List<CombatData> contributorsAvailable = combat.GetContributorsAvailable(combat.combatDataList);

        foreach (CombatData data in contributorsAvailable)
        {
            if(!contributions.ContainsKey(data.UnitAtacker))
            {
                contributions[data.UnitAtacker] = 0;
            }
            contributions[data.UnitAtacker] +=
                killerTypeBonus + 2 *
                data.DamageDealtByType.GetValueOrDefault(DamageType.Melee, 0f) +
                data.DamageDealtByType.GetValueOrDefault(DamageType.Ranged, 0f) +
                data.DamageDealtByType.GetValueOrDefault(DamageType.Magic, 0f) +
                data.DamageTaken + 2 * data.EffectsApplied.Count + 1;

            Debug.Log("Contributor: " + data.UnitAtacker.name + " Contribution: " + contributions[data.UnitAtacker]);
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
            Debug.Log("Unit: " + unit.name + " gained " + Mathf.FloorToInt(expGain * (contributions[unit] / totalContribution)) + " EXP.");
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
