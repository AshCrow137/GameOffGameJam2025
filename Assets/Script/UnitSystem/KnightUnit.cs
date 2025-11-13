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
		// Adding +2 damage if moved >= 3 tiles
		// TODO: update system
		if (chargeActive && tilesRemain <= 2)
		{
			MeleeAttackDamage += chargeDamage;
		}

		base.Attack(targetUnit);
		MeleeAttackDamage = 15;
	}
}