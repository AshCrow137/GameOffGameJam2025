using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSystem
{
    private static List<Combat> combats = new List<Combat>();

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
            Debug.LogError("Combat not found for the attacked unit.");
            RegisterCombat(atacked);
            AddDamage(atacked, atacker, damageType, damageDealt, hasKilled);
        }
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

}
