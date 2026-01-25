using System.Collections.Generic;
using UnityEngine;

public static class UpgradeSystemManager
{
    public static UnitStats currentUnit;

    public static List<Upgrade> ChooseForUpgrade(UnitStats unitToUpgrade)
    {
        currentUnit = unitToUpgrade;
        int numberOfUpgradesToShow = 3;
        List<Upgrade> upgradesToShow = new List<Upgrade>();
        List<Upgrade> aux = new List<Upgrade>();

        foreach(Upgrade upgrade in unitToUpgrade.PossibleUpgrades)
        {
            aux.Add(upgrade);
        }

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
