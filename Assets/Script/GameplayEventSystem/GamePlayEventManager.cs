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
        GridCity city = currentKingdom.controlledCities[Random.Range(0, currentKingdom.controlledCities.Count - 1)];
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
        ////spawn position = um lugar aleatorio junto da cidade
        //Vector3Int spawnPosition ;

        //UnitSpawner.Instance.PlaceUnit(unitSpawned,)
        if (unitSpawned != null)
        {
            city.TryToSpawnUnitInCity(unitSpawned);
        }

        Debug.Log("SpanwUnit");
    }

    private void SpecialEvent()
    {
        Debug.Log("Special Event");
    }
}
