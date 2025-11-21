using UnityEngine;

public class MadmanUnit : BaseGridUnitScript
{
    public override void Initialize(BaseKingdom owner) 
    {  
        base.Initialize(owner);
        MeleeAttackDamage += (int)Mathf.Round(6 * (0.1f * this.Owner.GetMadnessLevel()));
    }
}