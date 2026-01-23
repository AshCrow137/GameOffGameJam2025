using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMovementRange", menuName = "Upgrades/IncreaseMovementRange")]
public class IncreaseMovementRange : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMovementDistance.ChangeBaseStat(base.GetCurrentUpgradeLevel(this)); // Example increment
        Debug.Log($"Increased movement range by {base.GetCurrentUpgradeLevel(this)}.");
    }
}
