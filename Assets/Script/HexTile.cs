using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Custom tile class for hexagonal tiles with state management
/// Uses TileData to dynamically return sprites based on state stored in HexTilemapManager
/// </summary>
[CreateAssetMenu(fileName = "HexTile", menuName = "Tilemap/Hex Tile")]
public class HexTile : Tile
{
    public TileState defaultState = TileState.Available;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        
        HexTilemapManager manager = HexTilemapManager.Instance;
        TileState state = manager.GetTileState(position);
        tileData.color = manager.GetTileColor(state);
    }
}

