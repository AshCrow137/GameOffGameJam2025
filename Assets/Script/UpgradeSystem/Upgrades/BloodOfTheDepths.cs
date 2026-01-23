using UnityEngine;

[CreateAssetMenu(fileName = "BloodOfTheDepths", menuName = "Upgrades/BloodOfTheDepths")]
public class BloodOfTheDepths : Upgrade
{
    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        Vector3Int unitPos = unitToApplyUpgrade.GetOwner().GetCellPosition();
        if (HexTilemapManager.Instance.GetTileState(unitPos) == TileState.Water)
        {
            unitToApplyUpgrade.UnitHealth.CurrentHealth += base.GetCurrentUpgradeLevel(this);
            Debug.Log($"Increased health by {base.GetCurrentUpgradeLevel(this)} due to being on water");
        }
    }
}
