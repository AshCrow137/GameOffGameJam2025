using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMeleeAttack", menuName = "Upgrades/IncreaseMeleeAttack")]
public class IncreaseMeleeAttack : Upgrade
{
    public override void Init(Upgrade upgrade)
    {
        base.Init(upgrade);
    }

    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDamage.ChangeBaseStat(base.GetAmount(this)); // Example increment
        Debug.Log($"Increased attack power by {base.GetAmount(this)}.");
    }

}
