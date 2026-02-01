using System.Collections.Generic;
using UnityEngine;

public static class UpgradeSystemManager
{
    public static UnitStats currentUnit;

    /// <summary>
    /// Choose max 3 random upgrades for the unit to upgrade
    /// </summary>
    /// <param name="unitToUpgrade"></param>
    /// <returns></returns>
    public static List<Upgrade> ChooseForUpgrade(UnitStats unitToUpgrade)
    {
        currentUnit = unitToUpgrade;
        List<Upgrade> upgradesToShow = new List<Upgrade>();
        List<Upgrade> aux = new List<Upgrade>();

        //Filter upgrades that can be used by the unit
        foreach (Upgrade upgrade in unitToUpgrade.PossibleUpgrades)
        {
            if(CanAddUpgradeToShow(unitToUpgrade, upgrade))
            {
                aux.Add(upgrade);
            }
        }

        int numberOfUpgradesToShow = Mathf.Min(3, aux.Count);
        //Select Max 3 random upgrades to show
        for (int i = 0; i < numberOfUpgradesToShow; i++)
        {
            int randomIndex = Random.Range(0, aux.Count);
            upgradesToShow.Add(aux[randomIndex]);
            aux.RemoveAt(randomIndex);
        }

        return upgradesToShow;
    }
    /// <summary>
    /// Verify if the upgrade can be added to the list of upgrades to show
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="upgrade"></param>
    /// <returns></returns>
    private static bool CanAddUpgradeToShow(UnitStats unit, Upgrade upgrade)
    {
        if (upgrade.minimumLevelRequirement > unit.UnitExp.Level)
            return false;
        if (upgrade.upgradeLimitLevel != -1 && upgrade.GetCurrentUpgradeLevel(upgrade) >= upgrade.upgradeLimitLevel)
            return false;
        return true;
    }
}
