using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSpecialEventEnemy", menuName = "BaseGameplayEvent/SpawnSpecialEventEnemy")]
public class SpecialEventEnemy : SpawnSpecialEvents
{
    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        Debug.Log("Special Event Enemy");
        //choose a random city
        GridCity city = kingdom.GetControlledCities()[Random.Range(0, kingdom.GetControlledCities().Count - 1)];
        //get the possibles spawn positions from this city
        int possibleSpawnRange = 1;
        List<Vector3Int> possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, prefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        while (possibleSpawnPosition.Count == 0)
        {
            possibleSpawnRange++;
            possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, prefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        }
        //get the random spawn position
        Vector3Int spawnPosition = possibleSpawnPosition[Random.Range(0, possibleSpawnPosition.Count - 1)];
        //instanciate the unit
        UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition, kingdom);
    }
}