using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEvent", menuName = "BaseGameplayEvent/SpawnEvent")]
public class SpawnEvents : BaseGameplayEvent
{
    public GameObject meleePrefab;

    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        GridCity city = kingdom.GetControlledCities()[Random.Range(0, kingdom.GetControlledCities().Count - 1)];

        GameObject prefab = meleePrefab;

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

        if (kingdom is PlayerKingdom)
        {
            string text = "You Receive a Melee Unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }

    }
}