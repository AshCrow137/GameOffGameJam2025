using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class GamePlayEventManager : MonoBehaviour
{
    [SerializeField]
    private int ChanceToEvent = 3;


    [SerializeField]
    private BaseGameplayEvent[] gamePlayEventsPlayer;


    [SerializeField]
    private BaseGameplayEvent[] gamePlayEventsEnemy;

    public void Initialize()
    {
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
    }

    public void OnStartTurn(BaseKingdom kingdom)
    {
        int chance = Random.Range(1, ChanceToEvent);
        Debug.Log($"The chance is {chance}");

        BaseGameplayEvent gameplayEvent = null;

        if(kingdom is PlayerKingdom)
        {
            gameplayEvent = gamePlayEventsPlayer[Random.Range(0, gamePlayEventsPlayer.Length)];
        }
        else
        {
            gameplayEvent = gamePlayEventsEnemy[Random.Range(0, gamePlayEventsEnemy.Length)];
        }

        if (chance <= gameplayEvent.GetEventChance())
        {
            gameplayEvent.ExecuteEvent(kingdom);
        }
    }
}
