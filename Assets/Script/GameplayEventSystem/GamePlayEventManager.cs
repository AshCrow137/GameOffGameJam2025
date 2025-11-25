using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "TestEvent", menuName = "Scriptable Objects/TestEvent")]
public class TestEvent:ScriptableObject
{
    public int chance = 3;
    public virtual void ExecuteEvent()
    {

    }

}
[CreateAssetMenu(fileName = "SpawnUnitEvent", menuName = "Scriptable Objects/SpawnUnitEvent")]
public class SpawnUnitEvent:TestEvent
{
    public GameObject prefab;
    ///
    public override void ExecuteEvent()
    {
        base.ExecuteEvent();

    }
}

public class GamePlayEventManager : MonoBehaviour
{
    [SerializeField]
    private int ChanceToEvent = 3;
    [SerializeField]
    private GameObject playerMeleeUnit;
    [SerializeField]
    private GameObject enemyMeleeUnit;

    [SerializeField]
    private GameObject knightUnit;
    [SerializeField]
    private GameObject madmanUnit;

    private BaseKingdom currentKingdom;
    [SerializeField]
    private List<TestEvent> testEvents = new List<TestEvent>();

    private GamePlayEvent[] gamePlayEvents = {GamePlayEvent.GainResource,
                                              GamePlayEvent.LostResource,
                                              GamePlayEvent.SpawnUnit,
                                              GamePlayEvent.SpecialEvent};

    private Dictionary<GamePlayEvent, Action> chosenEvents;

    public void Initialize()
    {
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);

        chosenEvents = new Dictionary<GamePlayEvent, Action>
        {
            {GamePlayEvent.GainResource, GainResource },
            {GamePlayEvent.LostResource, LostResource },
            {GamePlayEvent.SpawnUnit, SpanwUnit },
            {GamePlayEvent.SpecialEvent, SpecialEvent }
        };
    }

    public void OnStartTurn(BaseKingdom kingdom)
    {
        currentKingdom = kingdom;
        int chance = Random.Range(1, 100);
        Debug.Log($"The chance is {chance}");
        if (chance <= ChanceToEvent)
        {
            chosenEvents[gamePlayEvents[Random.Range(0, gamePlayEvents.Length)]].Invoke();
        }
    }

    private bool isPlayerTurn()
    {
        PlayerKingdom pk = currentKingdom as PlayerKingdom;
        if(pk != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GainResource()
    {
        int gainValue = Random.Range(50, 80);
        ResourceType resource = ResourceType.Materials;
        switch(Random.Range(1, 3))
        {
            case 1:
                resource = ResourceType.Magic;
                break;
            case 2:
                resource = ResourceType.Gold;
                break;
            case 3:
                resource = ResourceType.Materials;
                break;
        }
        currentKingdom.Resources().AddAll(new Dictionary<ResourceType, int> { { resource, gainValue } });
        if (isPlayerTurn())
        {
            string text = $"You Receive {gainValue} of {resource}.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }
        Debug.Log($"Gain {gainValue} Resource");
    }

    private void LostResource()
    {
        int lostValue = Random.Range(30, 50);
        ResourceType resource = ResourceType.Materials;
        switch (Random.Range(1, 3))
        {
            case 1:
                resource = ResourceType.Magic;
                break;
            case 2:
                resource = ResourceType.Gold;
                break;
            case 3:
                resource = ResourceType.Materials;
                break;
        }
        currentKingdom.Resources().Remove(new Dictionary<ResourceType, int> { { resource, lostValue } });
        if (isPlayerTurn())
        {
            string text = $"You Lost {lostValue} of {resource}.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }
        Debug.Log($"Lost {lostValue} Resource");
    }

    private void SpanwUnit()
    {
        Debug.Log("SpanwUnit");
        GridCity city = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];

        GameObject unitSpawned = null;

        if (isPlayerTurn())
        {
            unitSpawned = playerMeleeUnit;

            string text = "You Receive a Melee Unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }
        else
        {
            unitSpawned = enemyMeleeUnit;
        }

        if (unitSpawned != null)
        {
            //Get random city
            GridCity currentCity = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];
            //get the possibles spawn positions from this city
            int possibleSpawnRange = 1;
            List<Vector3Int> possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, unitSpawned.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            while (possibleSpawnPosition.Count == 0)
            {
                possibleSpawnRange++;
                possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, unitSpawned.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            }
            //get the random spawn position
            Vector3Int spawnPosition = possibleSpawnPosition[Random.Range(0, possibleSpawnPosition.Count - 1)];
            //instanciate the unit
            UnitSpawner.Instance.PlaceUnit(unitSpawned, spawnPosition);
        }

    }

    private void SpecialEvent()
    {
        Debug.Log("Special Event");
        //who is the turn

        //player
        if (isPlayerTurn())
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
        List<Vector3Int> rangeSix = HexTilemapManager.Instance.GetCellsInRange(city.position, 6, madmanUnit.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        List<Vector3Int> possibleSpawnPositions = HexTilemapManager.Instance.GetCellsInRange(city.position, 10, madmanUnit.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());

        foreach (Vector3Int pos in rangeSix)
        {
            possibleSpawnPositions.Remove(pos);
        }

        Vector3Int spawnPosition = possibleSpawnPositions[Random.Range(0, possibleSpawnPositions.Count - 1)];

        UnitSpawner.Instance.PlaceUnit(madmanUnit, spawnPosition);

        string text = "You Receive a MadMan Unit.";
        UIManager.Instance.ShowGamePlayEvent(text);
    }

    private void SpecialEventEnemy(GridCity city)
    {
        //get the possibles spawn positions from this city
        int possibleSpawnRange = 1;
        List<Vector3Int> possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, knightUnit.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        while (possibleSpawnPosition.Count == 0)
        {
            possibleSpawnRange++;
            possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(city.position, possibleSpawnRange, knightUnit.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        }
        //get the random spawn position
        Vector3Int spawnPosition = possibleSpawnPosition[Random.Range(0, possibleSpawnPosition.Count - 1)];
        //instanciate the unit
        UnitSpawner.Instance.PlaceUnit(knightUnit, spawnPosition);
    }
}
