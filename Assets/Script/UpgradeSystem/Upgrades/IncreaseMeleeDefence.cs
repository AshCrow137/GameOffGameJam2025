using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDefence", menuName = "Upgrades/IncreaseDefence")]
public class IncreaseMeleeDefence : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDefence.ChangeBaseStat(base.amountToUpgrade); // Example increment
        Debug.Log($"Increased defence by {base.amountToUpgrade}.");
    }
}
