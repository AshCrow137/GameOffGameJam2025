using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttack", menuName = "Upgrades/IncreaseAttack")]
public class IncreaseMeleeAttack : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDamage.ChangeBaseStat(base.amountToUpgrade); // Example increment
        Debug.Log($"Increased attack power by {base.amountToUpgrade}.");
    }
    
}
