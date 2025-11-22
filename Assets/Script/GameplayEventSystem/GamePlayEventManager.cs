using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GamePlayEventManager : MonoBehaviour
{
    [SerializeField]
    private int ChanceToEvent = 3;
    [SerializeField]
    private GameObject playerMeleeUnit;
    [SerializeField]
    private GameObject enemyMeleeUnit;

    private BaseKingdom currentKingdom;

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
        if (chance <= ChanceToEvent)
        {
            chosenEvents[gamePlayEvents[Random.Range(0, gamePlayEvents.Length)]].Invoke();
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
        Resource.Instance.AddAll(new Dictionary<ResourceType, int> { { resource, gainValue } });
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
        Resource.Instance.Remove(new Dictionary<ResourceType, int> { { resource, lostValue } });
        Debug.Log($"Lost {lostValue} Resource");
    }

    private void SpanwUnit()
    {
        Debug.Log("SpanwUnit");
        GridCity city = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];
        PlayerKingdom pk = currentKingdom.gameObject.GetComponent<PlayerKingdom>();
        AIKingdom bot = currentKingdom.gameObject.GetComponent<AIKingdom>();

        GameObject unitSpawned = null;

        if (pk != null)
        {
            unitSpawned = playerMeleeUnit;
        }

        if (bot != null)
        {
            unitSpawned = enemyMeleeUnit;
        }

        if (unitSpawned != null)
        {
            //Get random city
            GridCity currentCity = currentKingdom.GetControlledCities()[Random.Range(0, currentKingdom.GetControlledCities().Count - 1)];
            //get the possibles spawn positions this city
            List<Vector3Int> possibleSpawnPosition = HexTilemapManager.Instance.GetCellsInRange(currentCity.position, 1, unitSpawned.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            //get the random spawn position
            Vector3Int spawnPosition = possibleSpawnPosition[Random.Range(0, possibleSpawnPosition.Count - 1)];
            //instanciate the unit
            UnitSpawner.Instance.PlaceUnit(unitSpawned, spawnPosition);
        }

    }

    private void SpecialEvent()
    {
        Debug.Log("Special Event");
        PlayerKingdom pk = currentKingdom as PlayerKingdom;
        AIKingdom bot = currentKingdom as AIKingdom;

        if (pk != null)
        {
            Debug.Log("Special Event Player");
        }

        if(bot != null)
        {
            Debug.Log("Special Event Enemy");
        }
    }

    private void SpecialEventPlayer()
    {

    }

    private void SpecialEventEnemy()
    {

    }
}
