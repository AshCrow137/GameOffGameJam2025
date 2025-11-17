using UnityEngine;
using System.Collections.Generic;

public class WavecallerUnit : BaseGridUnitScript
{
    [SerializeField]
    private int transformProgress = 0;
    private List<TileState> possibleTileStates = new List<TileState> { TileState.Land };


    public override void OnEntitySelect(BaseKingdom selector)
    {
        base.OnEntitySelect(selector);
        GameplayCanvasManager.instance.ActivateWavecallerButton(this);
        GlobalEventManager.OnTileClickEvent.RemoveListener(OnTileClicked);
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        GameplayCanvasManager.instance.DeactivateWavecallerButton();
    }

    public override void SpecialAbility()
    {
        List<Vector3Int>  possibleCellsInRage = HexTilemapManager.Instance.GetCellsInRange(GetCellPosition(), specialAbilityRange, possibleTileStates);
        Debug.Log(possibleCellsInRage);
    }
}