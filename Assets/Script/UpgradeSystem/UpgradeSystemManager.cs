using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeSystemManager
{
    public static List<Upgrade> ShowUpgradeOptions(UnitStats unitToUpgrade)
    {
        int numberOfUpgradesToShow = 3;
        List<Upgrade> upgradesToShow = new List<Upgrade>();
        List<Upgrade> aux = unitToUpgrade.PossibleUpgrades;

        for (int i = 0; i < numberOfUpgradesToShow; i++)
        {
            if (aux.Count == 0)
                break;
            int randomIndex = Random.Range(0, aux.Count);
            upgradesToShow.Add(aux[randomIndex]);
            aux.RemoveAt(randomIndex);
        }

        return upgradesToShow;
    }


}
