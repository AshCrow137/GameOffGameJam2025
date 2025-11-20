using UnityEngine;

public class InfiltratorUnit : BaseGridUnitScript
{
    [SerializeField]
    private int madnessAmount = 3;
    private bool isInCity = false;
    private GridCity infiltratedCity;

    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (isInCity)
        {
            infiltratedCity.GetOwner().IncreaseMadness(madnessAmount);
        }
    }

    protected override void OnTileClicked(HexTile tile, Vector3Int cellPos)
    {
        BaseGridUnitScript targetedUnit = hTM.GetUnitOnTile(cellPos);
        GridCity city = hTM.GetCityOnTile(cellPos);
        if (targetedUnit != null)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"Infiltrator cannot attack!");
        }
        else if (city != null)
        {
            TryToMoveUnitToTile(cellPos);
            isInCity = true;
            infiltratedCity = city;
        }
        else
        {
            TryToMoveUnitToTile(cellPos);
            isInCity = false;
            infiltratedCity = null;
        }

    }
}