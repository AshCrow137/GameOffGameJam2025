using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//Class created to store the combat data of an attacked unit.
public class Combat
{
    public UnitStats unitAtacked { get; private set; }
    public List<CombatData> combatDataList { get; private set; }

    public void Initialize(UnitStats unitAtacked)
    {
        this.unitAtacked = unitAtacked;
        combatDataList = new List<CombatData>();
    }

    //This method adds the damage caused by an attacker to the attacked unit.
    //if the unit has not yet attacked, it creates a new combatData.
    public void AddDamage(UnitStats unitAtacker, DamageType damageType, float damageDealt, bool hasKilled, float counterAtack, int currentTurn)
    {
        CombatData existingData = combatDataList.Find(data => data.UnitAtacker == unitAtacker);
        if (existingData != null)
        {
            existingData.DamageDealtByType[damageType] += damageDealt;
            existingData.HasKilled = existingData.HasKilled || hasKilled;
            existingData.DamageTaken += counterAtack;
            existingData.LastInteractTurn = currentTurn;
            if(hasKilled)
            {
                combatDataList = SortCombatData(combatDataList);
            }
        }
        else
        {
            CombatData newData = new CombatData(unitAtacker, damageType, damageDealt, hasKilled, counterAtack, currentTurn);
            combatDataList.Add(newData);
        }
    }

    public void UnitKilled(UnitStats unitAtacked)
    {
        if(unitAtacked == this.unitAtacked)
        {
            ExperienceSystem.Instance.DistributeExperience(this);
        }
    }

    public List<CombatData> GetContributorsAvailable(List<CombatData> combatDataFull)
    {
        List<CombatData> contributorsAvailable = new List<CombatData>();
        foreach (CombatData data in combatDataList)
        {
            if(data.LastInteractTurn >= TurnManager.instance.currentTurnCount - 4)
            {
                contributorsAvailable.Add(data);
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
}
