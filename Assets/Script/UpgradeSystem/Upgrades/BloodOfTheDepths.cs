using UnityEngine;

public class BloodOfTheDepths : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        Vector3Int unitPos = unitToApplyUpgrade.GetOwner().GetCellPosition();
        if (HexTilemapManager.Instance.GetTileState(unitPos) == TileState.Water)
        {
            unitToApplyUpgrade.UnitHealth.CurrentHealth += base.amountToUpgrade;
            Debug.Log($"Increased health by {base.amountToUpgrade} due to being on water");
        }
    }
}
