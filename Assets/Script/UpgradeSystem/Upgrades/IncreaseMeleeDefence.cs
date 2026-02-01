using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMeleeDefence", menuName = "Upgrades/IncreaseMeleeDefence")]
public class IncreaseMeleeDefence : Upgrade
{
    public override void Init(Upgrade upgrade)
    {
        base.Init(upgrade);
    }

    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDefence.ChangeBaseStat(base.GetAmount(this)); // Example increment
        Debug.Log($"Increased defence by {base.GetAmount(this)}.");
    }
}
