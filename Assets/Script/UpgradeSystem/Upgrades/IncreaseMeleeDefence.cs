using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMeleeDefence", menuName = "Upgrades/IncreaseMeleeDefence")]
public class IncreaseMeleeDefence : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDefence.ChangeBaseStat(base.GetCurrentUpgradeLevel(this)); // Example increment
        Debug.Log($"Increased defence by {base.GetCurrentUpgradeLevel(this)}.");
    }
}
