using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GiantWaveEvent", menuName = "BaseGameplayEvent/GiantWaveEvent")]
public class GiantWaveEvent : BaseGameplayEvent
{
    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        HexTilemapManager hexManager = HexTilemapManager.Instance;
        
        // Step 1 & 2: Find all land tiles adjacent to water tiles
        List<Vector3Int> coastalTiles = hexManager.GetAllCoastalLandTiles();
        
        if (coastalTiles.Count == 0)
        {
            Debug.LogWarning("No coastal tiles found for Giant Wave event");
            return;
        }
        
        // Step 3: Choose a random coastal land tile as the origin
        Vector3Int origin = coastalTiles[Random.Range(0, coastalTiles.Count)];
        
        Debug.Log($"Giant Wave origin at: {origin}");

        List<TileState> allStates = new List<TileState>
        {
            TileState.Land,
            TileState.OccuppiedByBuilding,
            TileState.OccupiedByUnit,
        };
        
        // Step 4: Get all tiles within radius 3 from origin
        List<Vector3Int> affectedTiles = hexManager.GetCellsInRange(origin, 3, allStates);
        
        // Step 5: Process each tile in radius
        foreach (Vector3Int tilePos in affectedTiles)
        {
            if (tilePos == new Vector3Int(-4, -4, 0))
            {
                Debug.Log("Hey");
            }
            // Check each entity and destroy if it cannot stand on water
            foreach (BaseGridEntity entity in hexManager.FindAllEntitiesAtPosition(tilePos))
            {
                if (!entity.GetCanStandOnTiles().Contains(TileState.Water))
                {
                    Debug.Log($"Giant Wave destroying {entity.gameObject.name} at {tilePos}");
                    entity.Death();
                }
            }
            
            // Convert tile to water
            hexManager.ChangeTile(tilePos, TileState.Water);
        }
        
        // Step 6: Show UI message for player kingdom\
        UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");

        // if (kingdom is PlayerKingdom)
        // {
        //     UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");
        // }
        
        Debug.Log($"Giant Wave event completed. Affected {affectedTiles.Count} tiles.");
    }
}


