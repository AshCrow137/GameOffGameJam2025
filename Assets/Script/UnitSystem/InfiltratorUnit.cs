using UnityEngine;

public class InfiltratorUnit : BaseGridUnitScript
{
    [SerializeField]
    private int madnessAmount = 3;
    private bool isInCity = false;
    private GridCity infiltratedCity;

    // TODO: Add stealth mechanic and fix madness increase to be each round,not each turn
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
            animator.SetBool("inCity", true);
        }
        else
        {
            TryToMoveUnitToTile(cellPos);
            isInCity = false;
            infiltratedCity = null;
            animator.SetBool("inCity", false);
        }

    }
}