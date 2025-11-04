using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Custom tile class for hexagonal tiles with state management
/// Uses TileData to dynamically return sprites based on state stored in HexTilemapManager
/// </summary>
[CreateAssetMenu(fileName = "HexTile", menuName = "Tilemap/Hex Tile")]
public class HexTile : Tile
{
    public TileState defaultState = TileState.Land;
    public TileState state { get; private set; }
    public TileState temporaryState {  get; private set; }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        // Set Tile colour based on state
        HexTilemapManager manager = HexTilemapManager.Instance;
        if(manager == null)
            return;
        state = manager.GetTileState(position);
        tileData.color = manager.GetTileColor(state);

        // Set Tile sprite based on building at position
        BuildingManager buildingManager = BuildingManager.Instance;
        if(buildingManager == null)
        {
            return;
        }
        Sprite sprite = buildingManager.GetBuildingSprite(position);
        if(sprite != null){
            tileData.sprite = sprite;
        }
    }
    
}

