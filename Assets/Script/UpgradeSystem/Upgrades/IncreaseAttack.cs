using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttack", menuName = "Scriptable Objects/Upgrades/IncreaseAttack")]
public class IncreaseAttack : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        unitToApplyUpgrade.UnitMeleeDamage.ChangeBaseStat(5); // Example increment
        Debug.Log("Increased attack power by 5.");
    }
    
}
