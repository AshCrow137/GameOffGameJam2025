using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMeleeAttack", menuName = "Upgrades/IncreaseMeleeAttack")]
public class IncreaseMeleeAttack : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDamage.ChangeBaseStat(base.GetCurrentUpgradeLevel(this)); // Example increment
        Debug.Log($"Increased attack power by {base.GetCurrentUpgradeLevel(this)}.");
    }
    
}
