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
    public bool clickLock = false;
    public float clickLockTime = 0.1f;

    public override void OnEntitySelect(BaseKingdom selector)
    {
        base.OnEntitySelect(selector);
        GameplayCanvasManager.instance.ActivateWavecallerButton(this);
        GlobalEventManager.OnTileClickEvent.AddListener(OnTileClicked);
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        GameplayCanvasManager.instance.DeactivateWavecallerButton();
        if (unitMode == UnitMode.Casting && transformProgress <= 2) { HPImage.color = Color.yellow; }
    }

    protected virtual void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (unitMode == UnitMode.Casting && transformProgress <= 2)
        {
            HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(transformingTile, MarkerColor.Blue);
            transformProgress++;
            MovementDistance = 0;
            HPImage.color = Color.yellow;
        }
        else
        {
            TransformTile(transformingTile);
            HexTilemapManager.Instance.RemoveAllMarkers();
            transformProgress = 0;
            unitMode = UnitMode.None;
            HPImage.color = Owner.GetKingdomColor();
        }
    }

    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
    }

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
            StartCoroutine(LockClicks());
            if (unitMode == UnitMode.Aiming)
            {
                Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
                if ((HexTilemapManager.Instance.GetTileState(mousePosition) != TileState.Land) || !possibleCellsInRange.Contains(mousePosition))
                {
                    GlobalEventManager.InvokeShowUIMessageEvent($"Wrong tile!");
                    return;
                }
                transformingTile = mousePosition;
                transformProgress = 1;
                unitMode = UnitMode.Casting;
                HPImage.color = Color.yellow;
                MovementDistance = 0;
            }
        }
        Debug.Log(possibleCellsInRange.Count);
        Debug.Log(unitMode);
    }
    private IEnumerator LockClicks()
    {
        clickLock = true;
        yield return new WaitForSeconds(clickLockTime);
        clickLock = false;
    }

    protected void TransformTile(Vector3Int tile)
    {
        HexTilemapManager.Instance.SetTileState(tile, TileState.Water);
        Debug.Log("Tile " + tile + " transformed by " + this);
    }

    private void OnTileClicked()
    {
        if (unitMode == UnitMode.Aiming)
        {
            Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
            if ((HexTilemapManager.Instance.GetTileState(mousePosition) != TileState.Land) || !possibleCellsInRange.Contains(mousePosition))
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"Wrong tile!");
                return;
            }
            transformingTile = mousePosition;
            transformProgress = 1;
            unitMode = UnitMode.Casting;
            HPImage.color = Color.yellow;
            MovementDistance = 0;
        }
    }
}