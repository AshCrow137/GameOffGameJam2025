using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    public int currentTurnCount { get; private set; } = 1;

    private List<GameObject> turnOrder; //player and enemy turn order
    private int currentOrderIndex; //player

    public TurnManager(List<GameObject> turnOrder)
    {
        this.turnOrder = turnOrder;
    }

    public void Initialize()
    {
        currentOrderIndex = 0;
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

    public void OnTurnStart(GameObject entity)
    {
        //camera focus on the current Player/Unit
        Debug.Log($"Turn {currentTurnCount} Start: {entity.name}'s turn.");

        //Every object whose turn needs to be handled should have a EntityTurnHandler's subclass component.
        entity.GetComponent<EntityTurnHandler>()?.OnTurnStart();

    }

    public void OnTurnEnd()
    {
        //do something at the end of the turn
        currentOrderIndex++;
        if (currentOrderIndex >= turnOrder.Count)
        {
            currentOrderIndex = 0;
        }

        Debug.Log($"Turn {currentTurnCount} End: {turnOrder[currentOrderIndex].name}'s turn.");
        //when finish the turn, call NextTurn
        NextTurn();
    }

    //public function to call from button press
    public void NextTurn()
    {
        Debug.Log("Next Turn");
        //increment turn count
        currentTurnCount++;

        //call OnTurnStart to start the next turn
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

}
