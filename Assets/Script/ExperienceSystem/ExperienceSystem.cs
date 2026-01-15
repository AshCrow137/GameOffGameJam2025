using System.Collections.Generic;
using UnityEngine;

public static class ExperienceSystem
{
    private static float expGain;

    private static Dictionary<UnitStats, float> contributions;

    public static void DistributeExperience(Combat combat)
    {
        Debug.Log("Total Experience to distribute calculated.");
        expGain = TotalExperience(combat.unitAtacked);
        Debug.Log("Experience to distribute: " + expGain);

        CalculateContributions(combat);

        DistributionExp(contributions);
    }

    private static float TotalExperience(UnitStats unitKilled)
    {
        return unitKilled.UnitExp.ExpModifier * (1 + 0.1f * unitKilled.UnitExp.Level);
    }

    private static void CalculateContributions(Combat combat)
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
            float meleeDamage = data.DamageDealtByType.GetValueOrDefault(DamageType.Melee, 0f);
            float rangedDamage = data.DamageDealtByType.GetValueOrDefault(DamageType.Ranged, 0f);
            float damageTaken = BattleSystem.DamageTakingTo(data.UnitAtacker, combat.unitAtacked);
            Debug.Log("Damage Taken by " + data.UnitAtacker.name + ": " + damageTaken);
            int EffectsApplied = data.UnitAtacker.GetOwner().activeEffects.Count;

            contributions[data.UnitAtacker] += killerTypeBonus + 2 * 
                meleeDamage + rangedDamage + damageTaken + 2 * EffectsApplied + 1;

            Debug.Log("Contributor: " + data.UnitAtacker.name + " Contribution: " + contributions[data.UnitAtacker]);
        }
    }

    private static void DistributionExp(Dictionary<UnitStats, float> contributors)
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

    private static float CalculateKilledTypeBonus(CombatData data)
    {
        if(data.KilledTypeDamage == DamageType.Melee)
        {
            return 20;
        }
        else
        {
            return 10;
        }
    }

    public static int ExpToNextLevel(UnitStats unit)
    {
        int level = unit.UnitExp.Level;
        int expMod = unit.UnitExp.ExpModifier;

        int expToNextLevel = 10 * (level + 1) * expMod / 100;
        Debug.Log($"Experience of {unit.name} is: {expToNextLevel}");
        return expToNextLevel;
    }
}
