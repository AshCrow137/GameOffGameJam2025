using UnityEngine;

public class IncreaseMovementRange : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMovementDistance.ChangeBaseStat(base.amountToUpgrade); // Example increment
        Debug.Log($"Increased movement range by {base.amountToUpgrade}.");
    }
}
