using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//Class created to store the combat data of an attacked unit.
public class Combat
{
    public UnitStats unitAtacked { get; private set; }
    public List<CombatData> combatDataList { get; private set; }

    public Combat(UnitStats unitAtacked)
    {
        this.unitAtacked = unitAtacked;
        combatDataList = new List<CombatData>();
        Debug.Log("New Combat created for unit: " + unitAtacked.name);
    }

    //This method adds the damage caused by an attacker to the attacked unit.
    //if the unit has not yet attacked, it creates a new combatData.
    public void AddDamageToAtacker(UnitStats unitAtacker, DamageType damageType, float damageDealt, bool hasKilled, int currentTurn)
    {
        CombatData existingData = combatDataList.Find(data => data.UnitAtacker == unitAtacker);
        if (existingData != null)
        {
            if(!existingData.DamageDealtByType.ContainsKey(damageType))
            {
                existingData.DamageDealtByType[damageType] = 0;
            }
            existingData.DamageDealtByType[damageType] += damageDealt;
            existingData.HasKilled = existingData.HasKilled || hasKilled;
            existingData.LastInteractTurn = currentTurn;
            if(hasKilled)
            {
                combatDataList = SortCombatData(combatDataList);
            }
        }
        else
        {
            CombatData newData = new CombatData(unitAtacker, damageType, damageDealt, hasKilled, currentTurn);
            combatDataList.Add(newData);
        }
    }

    public List<CombatData> GetContributorsAvailable(List<CombatData> combatDataFull)
    {
        List<CombatData> contributorsAvailable = new List<CombatData>();
        foreach (CombatData data in combatDataList)
        {
            ////Test Version
            //if(data.LastInteractTurn > 5 - 4)
            //{
            //    contributorsAvailable.Add(data);
            //}
            //Production Version
            if(data.UnitAtacker.GetOwner().GetCurrentHealth() > 0)
            {
                if (data.LastInteractTurn > TurnManager.instance.currentTurnCount - 4)
                {
                    contributorsAvailable.Add(data);
                }
            }
        }

        return contributorsAvailable;

    }

    private List<CombatData> SortCombatData(List<CombatData> dataList)
    {
        CombatData dataAux;

        for(int i = 0; i < dataList.Count; i++)
        {
            if(dataList[i].HasKilled)
            {
                dataAux = dataList[i];
                dataList.RemoveAt(i);
                dataList.Insert(0, dataAux);
            }
        }

        return dataList;
    }

    public void AddEffectFromAtacker(UnitStats unitAtacker, BaseEffect effect)
    {
        CombatData existingData = combatDataList.Find(data => data.UnitAtacker == unitAtacker);
        if (existingData != null)
        {
            existingData.AddEffect(existingData, effect);
        }
    }

    public void RemoveEffectFromAtacker(UnitStats unitAtacker, BaseEffect effect)
    {
        CombatData existingData = combatDataList.Find(data => data.UnitAtacker == unitAtacker);
        if (existingData != null)
        {
            existingData.RemoveEffect(existingData, effect);
        }
    }
}
