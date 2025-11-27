using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSpecialEventPlayer", menuName = "BaseGameplayEvent/SpawnSpecialEventPlayer")]
public class SpecialEventPlayer : SpawnSpecialEvents
{

    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        BaseKingdom currentKingdom = kingdom;
        Debug.Log("Special Event Player");
        List<BaseKingdom> baseKingdoms = TurnManager.instance.GetKingdoms();
        List<AIKingdom> aiKingdoms = new List<AIKingdom>();
        foreach (BaseKingdom bKingdom in baseKingdoms)
        {
            if (bKingdom is AIKingdom) aiKingdoms.Add(bKingdom as AIKingdom);
        }
        AIKingdom randomKingdom = aiKingdoms[Random.Range(0, aiKingdoms.Count - 1)];
        GridCity randomCity = randomKingdom.GetControlledCities()[Random.Range(0, randomKingdom.GetControlledCities().Count - 1)];
        List<Vector3Int> rangeSix = HexTilemapManager.Instance.GetCellsInRange(randomCity.position, 6, prefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        List<Vector3Int> possibleSpawnPositions = HexTilemapManager.Instance.GetCellsInRange(randomCity.position, 10, prefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        foreach (Vector3Int pos in rangeSix)
        {
            possibleSpawnPositions.Remove(pos);
        }

        Vector3Int spawnPosition = possibleSpawnPositions[Random.Range(0, possibleSpawnPositions.Count - 1)];

        UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition, kingdom);

        string text = "You Receive a MadMan Unit.";
        UIManager.Instance.ShowGamePlayEvent(text);
    }
}