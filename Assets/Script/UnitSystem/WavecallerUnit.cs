using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

public class WavecallerUnit : BaseGridUnitScript
{
    [SerializeField]
    private int transformProgress = 0;
    private List<TileState> possibleTileStates = new List<TileState> { TileState.Land,TileState.OccupiedByUnit,TileState.OccuppiedByBuilding,TileState.OccupiedByCity };
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
        if(unitMode !=UnitMode.Aiming)
        {
            GameplayCanvasManager.instance.DeactivateWavecallerButton();
        }
        
        //if (unitMode == UnitMode.Casting && transformProgress <= 2) { HPImage.color = Color.yellow; }
    }

    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (unitMode == UnitMode.Casting && transformProgress <= 4)
        {
            animator.SetBool("CastStart", true);
            //HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(transformingTile, MarkerColor.Blue);
            transformProgress++;
            MovementDistance = 0;
            //HPImage.color = Color.yellow;
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
        base.SpecialAbility();
        animator.SetBool("CastStart", false);
        animator.SetBool("CastFinish", false);
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
        HexTilemapManager.Instance.ChangeTile(tile, TileState.Water);

        Debug.Log("Tile " + tile + " transformed by " + this);
    }

    private void PerformTransformation()
    {
        animator.SetBool("CastStart", false);
        animator.SetBool("CastFinish", true);
        possibleCellsInRange = HexTilemapManager.Instance.GetCellsInRange(GetCellPosition(), specialAbilityRange, possibleTileStates);
        List<BaseGridEntity> entitiesToRemove = new List<BaseGridEntity>();
        foreach (Vector3Int tilePos in possibleCellsInRange)
        {
            if (tilePos == new Vector3Int(-4, -4, 0))
            {
                Debug.Log("Hey");
            }
            // Check each entity and destroy if it cannot stand on water
            foreach (BaseGridEntity entity in hTM.FindAllEntitiesAtPosition(tilePos))
            {
                if (!entity.GetCanStandOnTiles().Contains(TileState.Water))
                {
                    Debug.Log($"Giant Wave destroying {entity.gameObject.name} at {tilePos}");
                    //entity.Death();
                    entitiesToRemove.Add(entity);
                }
            }

        }
        foreach (BaseGridEntity entity in entitiesToRemove)
        {
            entity.Death();
        }
        foreach (Vector3Int pos in possibleCellsInRange)
        {
            TransformTile(pos);
        }
        
        HexTilemapManager.Instance.RemoveAllMarkers();
        transformProgress = 0;
        unitMode = UnitMode.None;
        //HPImage.color = Owner.GetKingdomColor();
        MovementDistance = 2;
        tilesRemain = MovementDistance;
        AttacksPerTurn = 1;
        //animator.SetBool("CastStart", false);
        //animator.SetBool("CastFinish", false);
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
        foreach (var cell in possibleCellsInRange) 
        {
            StartTransformation(cell);
        }
            
        aiming = false;
        
    }
}