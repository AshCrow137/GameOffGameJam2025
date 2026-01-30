using System.Collections.Generic;
using UnityEngine;

public static class UpgradeSystemManager
{
    public static UnitStats currentUnit;

    public static List<Upgrade> ChooseForUpgrade(UnitStats unitToUpgrade)
    {
        currentUnit = unitToUpgrade;
        List<Upgrade> upgradesToShow = new List<Upgrade>();
        List<Upgrade> aux = new List<Upgrade>();

        foreach(Upgrade upgrade in unitToUpgrade.PossibleUpgrades)
        {
            if(CanAddUpgradeToShow(unitToUpgrade, upgrade))
            {
                aux.Add(upgrade);
            }
        }
        int numberOfUpgradesToShow = aux.Count;

        for (int i = 0; i < numberOfUpgradesToShow; i++)
        {
            int randomIndex = Random.Range(0, aux.Count);
            upgradesToShow.Add(aux[randomIndex]);
            aux.RemoveAt(randomIndex);
        }

        return upgradesToShow;
    }

    private static bool CanAddUpgradeToShow(UnitStats unit, Upgrade upgrade)
    {
        if (upgrade.minimumLevelRequirement > unit.UnitExp.Level)
            return false;
        if (upgrade.upgradeLimitLevel != -1 && upgrade.GetCurrentUpgradeLevel(upgrade) >= upgrade.upgradeLimitLevel)
            return false;
        return true;
    }
}
