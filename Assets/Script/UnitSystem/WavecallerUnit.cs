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
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        GameplayCanvasManager.instance.DeactivateWavecallerButton();
    }

    // Ability to transfrom land tile into water tile
    public override void SpecialAbility()
    {
        List<Vector3Int>  possibleCellsInRange = HexTilemapManager.Instance.GetCellsInRange(GetCellPosition(), specialAbilityRange, possibleTileStates);
        if ( possibleCellsInRange.Count == 0 )
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"No land tiles in range!");
            return;
        }
        else
        {
            GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
            foreach (Vector3Int pos in possibleCellsInRange)
            {
                HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Blue);
            }

            Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
            if ((HexTilemapManager.Instance.GetTileState(mousePosition) != TileState.Land) || !possibleCellsInRange.Contains(mousePosition))
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"Wrong tile!");
                return;
            }
            
            HexTilemapManager.Instance.SetTileState(mousePosition, TileState.Water);

        }
        Debug.Log(possibleCellsInRange.Count);
    }
}