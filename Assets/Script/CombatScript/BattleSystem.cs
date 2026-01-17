using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSystem
{
    private static List<Combat> combats = new List<Combat>();
    private static List<Combat> completeCombats = new List<Combat>();

    private static bool IsInCombat(UnitStats atacked)
    {
        foreach(Combat combat in combats)
        {
            if(combat.unitAtacked == atacked)
            {
                return true;
            }
        }
        return false;
    }

    private static void RegisterCombat(UnitStats Atacked)
    {
        if (!IsInCombat(Atacked))
        {
            combats.Add(new Combat(Atacked));
            Debug.Log("Battle registered for unit: " + Atacked.name);
        }
    }

    public static void AddDamage(UnitStats atacked, UnitStats atacker, DamageType damageType, float damageDealt, bool hasKilled)
    {
        Combat combat = combats.Find(c => c.unitAtacked == atacked);
        if (combat != null)
        {
            combat.AddDamageToAtacker(atacker, damageType, damageDealt, hasKilled, TurnManager.instance.currentTurnCount);
        }
        else
        {
            RegisterCombat(atacked);
            AddDamage(atacked, atacker, damageType, damageDealt, hasKilled);
        }
        Debug.Log($"Number Of Combats Active: {combats.Count}");
    }

    public static void UnitKilled(UnitStats unitAtacked)
    {
        Combat combat = combats.Find(c => c.unitAtacked == unitAtacked);
        if (combat != null)
        {
            Debug.Log("Distributing Experience");
            ExperienceSystem.DistributeExperience(combat);
        }
    }

    public static Combat GetCombat(UnitStats atacked)
    {
        return combats.Find(c => c.unitAtacked == atacked);
    }

    private static float CalculateDamageOf(Combat combat, UnitStats attacker)
    {
        CombatData data = combat.combatDataList.Find(d => d.UnitAtacker == attacker);
        if(data != null)
        {
            float totalDamage = 0f;
            foreach(float damage in data.DamageDealtByType.Values)
            {
                totalDamage += damage;
            }
            return totalDamage;
        }
        return 0f;
    }

    public static float DamageTakingTo(UnitStats attacker, UnitStats attacked)
    {
        Combat combat = GetCombat(attacker);
        if(combat != null)
        {
            return CalculateDamageOf(combat, attacked);
        }
        return 0f;
    }

    public static void AddEffect(UnitStats atacked, UnitStats atacker, BaseEffect effect)
    {
        Combat combat = combats.Find(c => c.unitAtacked == atacked);
        if (combat != null)
        {
            combat.AddEffectFromAtacker(atacker, effect);
        }
        else
        {
            RegisterCombat(atacked);
            combat = combats.Find(c => c.unitAtacked == atacked);
            combat.AddEffectFromAtacker(atacker, effect);
        }
    }

    public static void RemoveEffect(UnitStats atacked, UnitStats atacker, BaseEffect effect)
    {
        Combat combat = combats.Find(c => c.unitAtacked == atacked);
        if (combat != null)
        {
            combat.RemoveEffectFromAtacker(atacker, effect);
        }
    }

    public static int ActiveEffectsCountByAtacker(UnitStats atacked, CombatData atackerData)
    {
        Combat combat = combats.Find(c => c.unitAtacked == atacked);
        if (combat != null)
        {
            CombatData data = combat.combatDataList.Find(d => d == atackerData);
            if(data != null)
            {
                int count = 0;
                foreach (BaseEffect effect in atacked.GetOwner().activeEffects)
                {
                    for(int i = 0; i< data.EffectsApplied.Count; i++)
                    {
                        if(effect.Name == data.EffectsApplied[i])
                        {
                            count++;
                        }
                    }

                }
            }
        }
        return 0;
    }

    public static void EndCombat(Combat combat)
    {
        if(combats.Contains(combat))
        {
            combats.Remove(combat);
            completeCombats.Add(combat);
            Debug.Log("Combat ended for unit: " + combat.unitAtacked.name);
        }
    }

}
