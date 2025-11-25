using UnityEngine;

public class MadmanUnit : BaseGridUnitScript
{
    private int resultDamage;
    private int baseDamage;

    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        baseDamage = MeleeAttackDamage;
    }

    protected override void Attack(BaseGridEntity targetEntity)
    {
        MeleeAttackDamage += (int)Mathf.Round(6 * (0.1f * targetEntity.GetOwner().GetMadnessLevel()));
        base.Attack(targetEntity);
        MeleeAttackDamage = baseDamage;
    }
}