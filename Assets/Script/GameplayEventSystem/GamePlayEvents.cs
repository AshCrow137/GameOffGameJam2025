using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GamePlayEvents", menuName = "GamePlayEvents")]
public class GamePlayEvents : ScriptableObject
{
    public virtual void ExecuteEvent()
    {

    }
}

[CreateAssetMenu(fileName = "ResourceEvents", menuName = "GamePlayEvents/ResourceEvent")]
public class ResourceEvents : GamePlayEvents
{
    public List<ResourceType> resources;
    public bool isIncremented;
    public int minResource;
    public int maxResource;

    public override void ExecuteEvent()
    {
        int amount = Random.Range(minResource, maxResource);
        ResourceType resource = resources[Random.Range(0, resources.Count)];

        if(!isIncremented)
        {
            amount *= -1;
        }

        Resource.Instance.AddAll(new Dictionary<ResourceType, int> { { resource, amount } });

        Debug.Log($"Gain {amount} {resource}");

        if(TurnManager.instance.GetCurrentActingKingdom() is PlayerKingdom)
        {
            string text;
            if (isIncremented)
            {
                text = $"You Receive {amount} of {resource}.";
            }
            else
            {
                text = $"You Lost {Mathf.Abs(amount)} of {resource}.";
            }
                UIManager.Instance.ShowGamePlayEvent(text);
        }
    }
}

[CreateAssetMenu(fileName = "SpawnEvent", menuName = "GamePlayEvents/SpawnEvent")]
public class SpawnEvents : GamePlayEvents
{
    public GameObject PlayerMeleePrefab;
    public GameObject EnemyMeleePrefab;

    public override void ExecuteEvent()
    {
        BaseKingdom currentKingdom = TurnManager.instance.GetCurrentActingKingdom();
        GridCity city = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];

        GameObject prefab = currentKingdom is PlayerKingdom ? PlayerMeleePrefab : EnemyMeleePrefab;

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
        UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition);

        if (currentKingdom is PlayerKingdom)
        {
            string text = "You Receive a Melee Unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }

    }

    [CreateAssetMenu(fileName = "SpawnSpecialEvent", menuName = "GamePlayEvents/SpawnSpecialEvent")]
    public class SpawnSpecialEvents : GamePlayEvents
    {
        public GameObject PlayerMeleePrefab;
        public GameObject EnemyMeleePrefab;

        public override void ExecuteEvent()
        {
            BaseKingdom currentKingdom = TurnManager.instance.GetCurrentActingKingdom();
            //player
            if (currentKingdom is PlayerKingdom)
            {
                Debug.Log("Special Event Player");
                GameObject[] botKingdonsObjects = GameObject.FindGameObjectsWithTag("EnemyCity");
                GridCity botCity = botKingdonsObjects[Random.Range(0, botKingdonsObjects.Length - 1)].GetComponent<GridCity>();
                SpecialEventPlayer(botCity);
            }
            else
            {
                Debug.Log("Special Event Enemy");
                //choose a random city
                GridCity city = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];
                SpecialEventEnemy(city);
            }
        }

        private void SpecialEventPlayer(GridCity city)
        {
            List<Vector3Int> rangeSix = HexTilemapManager.Instance.GetCellsInRange(city.position, 6, PlayerMeleePrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            List<Vector3Int> possibleSpawnPositions = HexTilemapManager.Instance.GetCellsInRange(city.position, 10, PlayerMeleePrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());

            foreach (Vector3Int pos in rangeSix)
            {
                possibleSpawnPositions.Remove(pos);
            }

            Vector3Int spawnPosition = possibleSpawnPositions[Random.Range(0, possibleSpawnPositions.Count - 1)];

            UnitSpawner.Instance.PlaceUnit(PlayerMeleePrefab, spawnPosition);

            string text = "You Receive a MadMan Unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }

        private void SpecialEventEnemy(GridCity city)
        {
            //get the possibles spawn positions from this city
            int possibleSpawnRange = 1;
            List<Vector3Int> possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, EnemyMeleePrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            while (possibleSpawnPosition.Count == 0)
            {
                possibleSpawnRange++;
                possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, EnemyMeleePrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            }
            //get the random spawn position
            Vector3Int spawnPosition = possibleSpawnPosition[Random.Range(0, possibleSpawnPosition.Count - 1)];
            //instanciate the unit
            UnitSpawner.Instance.PlaceUnit(EnemyMeleePrefab, spawnPosition);
        }
    }
}