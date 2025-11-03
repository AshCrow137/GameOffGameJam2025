using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int currentTurnCount { get; private set; } = 1;
    [SerializeField]
    private List<GameObject> turnOrder; //player and enemy turn order
    private int currentOrderIndex; //player

    public void Initialize()
    {
        currentOrderIndex = 0;
        //turnOrder = new List<GameObject>();
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

    public void OnTurnStart(GameObject entity)
    {
        //camera focus on the current Player/Unit
        Debug.Log($"Turn {currentTurnCount} Start: {entity.name}'s turn.");

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
    private void NextTurn()
    {
        Debug.Log("Next Turn");
        //increment turn count
        currentTurnCount++;

        //call OnTurnStart to start the next turn
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

}
