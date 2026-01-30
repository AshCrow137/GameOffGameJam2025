using UnityEngine;

[CreateAssetMenu(fileName = "BloodOfTheDepths", menuName = "Upgrades/BloodOfTheDepths")]
public class BloodOfTheDepths : Upgrade
{
    public override void Init(Upgrade upgrade)
    {
        base.Init(upgrade);
    }

    public override void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        base.ApplyUpgrade(unitToApplyUpgrade);
        Vector3Int unitPos = unitToApplyUpgrade.GetOwner().GetCellPosition();
        //TODO: I need to check if tile is water
        if (HexTilemapManager.Instance.GetTileState(unitPos) == TileState.Water)
        {
            unitToApplyUpgrade.UnitHealth.CurrentHealth += base.GetAmount(this);
            Debug.Log($"Increased health by {base.GetAmount(this)} due to being on water");
        }
    }
}
