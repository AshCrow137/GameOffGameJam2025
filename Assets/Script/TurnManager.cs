using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    protected int currentTurnCount { get; private set; } = 1;

    private List<Entity> turnOrder = new List<Entity>(); //player and enemy turn order
    private int currentOrderIndex = 0; //player

    public TurnManager(List<Entity> entities)
    {
        this.turnOrder = entities;
    }

    public void Initialize()
    {
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

    public void OnTurnStart(Entity entity)
    {
        //camera focus on the current Player/Unit

    }

    public void OnTurnEnd()
    {
        //do something at the end of the turn
        currentOrderIndex++;
        if (currentOrderIndex >= turnOrder.Count)
        {
            currentOrderIndex = 0;
        }

        //when finish the turn, call NextTurn
        NextTurn();
    }
    private void NextTurn()
    {
        //increment turn count
        currentTurnCount++;

        //call OnTurnStart to start the next turn
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

}
