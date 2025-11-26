using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "BaseGameplayEvent", menuName = "BaseGameplayEvent")]
public class BaseGameplayEvent : ScriptableObject
{
    [SerializeField]
    protected int EventChance = 5;
    public virtual void ExecuteEvent(BaseKingdom kingdom )
    {

    }

    public int GetEventChance() => EventChance;
}

[CreateAssetMenu(fileName = "ResourceEvents", menuName = "BaseGameplayEvent/ResourceEvent")]
public class ResourceEvents : BaseGameplayEvent
{
    public List<ResourceType> resources;
    public bool isIncremented;
    public int minResource;
    public int maxResource;

    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        int amount = Random.Range(minResource, maxResource);
        ResourceType resource = resources[Random.Range(0, resources.Count)];

        if(!isIncremented)
        {
            amount *= -1;
        }

        Resource.Instance.AddAll(new Dictionary<ResourceType, int> { { resource, amount } });

        Debug.Log($"Gain {amount} {resource}");

        if(kingdom is PlayerKingdom)
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
        UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition);

        if (kingdom is PlayerKingdom)
        {
            string text = "You Receive a Melee Unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }

    }

    [CreateAssetMenu(fileName = "SpawnSpecialEvent", menuName = "BaseGameplayEvent/SpawnSpecialEvent")]
    public class SpawnSpecialEvents : BaseGameplayEvent
    {
        public GameObject prefab;

        public override void ExecuteEvent(BaseKingdom kingdom)
        {
            
        }

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

                UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition);

                string text = "You Receive a MadMan Unit.";
                UIManager.Instance.ShowGamePlayEvent(text);
            }
        }

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
                UnitSpawner.Instance.PlaceUnit(prefab, spawnPosition);
            }
        }
    }
}