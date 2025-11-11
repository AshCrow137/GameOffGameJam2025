using UnityEngine;

public class KnightUnit : BaseGridUnitScript
{
	[Header("Special ability")]
	[SerializeField]
	protected bool chargeActive = true;

	protected override void Attack(BaseGridUnitScript targetUnit)
	{
		int currentAttackDamage = AttackDamage;

		// Adding +2 damage if moved >= 3 tiles
		if (chargeActive && tilesRemain <= 2)
		{
			currentAttackDamage += 2;
		}

		// Multiplying damage by 1.5 if target is an archer
		if (targetUnit.GetUnitType() == UnitType.Archer)
		{
			currentAttackDamage = Mathf.RoundToInt(currentAttackDamage * 1.5f);
		}

		targetUnit.TakeDamage(currentAttackDamage);
		if (!CanMoveAfterattack)
		{
			tilesRemain = 0;
			remainMovementText.text = tilesRemain.ToString();
		}
	}
}