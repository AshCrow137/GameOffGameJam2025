using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Profiling.LowLevel;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class WavecallerUnit : BaseGridUnitScript
{
    [SerializeField]
    private int transformProgress = 0;
    private List<TileState> possibleTileStates = new List<TileState> { TileState.Land };
    private UnitMode unitMode = UnitMode.None;
    private Vector3Int transformingTile;
    private List<Vector3Int> possibleCellsInRange;

    public override void OnEntitySelect(BaseKingdom selector)
    {
        base.OnEntitySelect(selector);
        GameplayCanvasManager.instance.ActivateWavecallerButton(this);
        if (unitMode == UnitMode.Casting)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"Unit is casting and cannot move or attack");
        }
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        GameplayCanvasManager.instance.DeactivateWavecallerButton();
        if (unitMode == UnitMode.Casting && transformProgress <= 2) { HPImage.color = Color.yellow; }
    }

    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (unitMode == UnitMode.Casting && transformProgress <= 4)
        {
            HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(transformingTile, MarkerColor.Blue);
            transformProgress++;
            MovementDistance = 0;
            HPImage.color = Color.yellow;
        }
    }

    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
        if (unitMode == UnitMode.Casting && transformProgress == 5)
        {
            PerformTransformation();
        }
    }

    // Triggers after pressing ability button, returns if no land tiles in range, starts transformation if right tile is chosen
    public override void SpecialAbility()
    {
        possibleCellsInRange = HexTilemapManager.Instance.GetCellsInRange(GetCellPosition(), specialAbilityRange, possibleTileStates);
        if (possibleCellsInRange.Count == 0)
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"No land tiles in range!");
            return;
        }
        else
        {
            foreach (Vector3Int pos in possibleCellsInRange)
            {
                HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Blue);
            }
            unitMode = UnitMode.Aiming;
            aiming = true;
        }
    }

    protected void TransformTile(Vector3Int tile)
    {
        HexTilemapManager.Instance.SetTileState(tile, TileState.Water);
        Debug.Log("Tile " + tile + " transformed by " + this);
    }

    private void PerformTransformation()
    {
        TransformTile(transformingTile);
        HexTilemapManager.Instance.RemoveAllMarkers();
        transformProgress = 0;
        unitMode = UnitMode.None;
        HPImage.color = Owner.GetKingdomColor();
        MovementDistance = 2;
        tilesRemain = MovementDistance;
        AttacksPerTurn = 1;
    }

    private void StartTransformation(Vector3Int tile)
    {
        MovementDistance = 0;
        AttacksPerTurn = 0;
        tilesRemain = MovementDistance;
        remainMovementText.text = tilesRemain.ToString();
        transformingTile = tile;
        transformProgress = 1;
        unitMode = UnitMode.Casting;
        HPImage.color = Color.yellow;
    }

    public override void OnChosingTile()
    {
        base.OnChosingTile();
        //GlobalEventManager.InvokeShowUIMessageEvent($"test " + unitMode);
        
            Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
            if ((HexTilemapManager.Instance.GetTileState(mousePosition) != TileState.Land) || !possibleCellsInRange.Contains(mousePosition))
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"Wrong tile!");
                return;
            }

            StartTransformation(mousePosition);
        aiming = false;
        
    }
}