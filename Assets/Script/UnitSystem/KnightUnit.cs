using UnityEngine;

public class KnightUnit : BaseGridUnitScript
{
	[Header("Special ability")]
	[SerializeField]
	protected bool chargeActive = true;
	[SerializeField]
	protected int chargeDamage = 5;

    protected override void Attack(BaseGridUnitScript targetUnit)
	{
		// Adding +2 damage if distance travelled >= 3 tiles
		if (chargeActive && (int)Mathf.Round(distanceTravelled) >= 3)
		{
			MeleeAttackDamage += chargeDamage;
		}

		base.Attack(targetUnit);
		MeleeAttackDamage = 15;
	}

}