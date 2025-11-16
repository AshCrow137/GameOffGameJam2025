using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int currentTurnCount { get; private set; } = 1;
    [SerializeField]
    private List<BaseKingdom> turnOrder; //player and enemy turn order
    private int currentOrderIndex; //player
    public static TurnManager instance { get; private set; }

    [SerializeField]
    private TMP_Text turnText;

    //public TurnManager(List<GameObject> turnOrder)
    //{
    //    this.turnOrder = turnOrder;
    //}

    public void Initialize()
    {
        instance = this;
        currentOrderIndex = 0;
        OnTurnStart(turnOrder[currentOrderIndex]);
       
    }

    public void OnTurnStart(BaseKingdom entity)
    {
        //camera focus on the current Player/Unit
        //Debug.Log($"Turn {currentTurnCount} Start: {entity.name}'s turn.");
        turnText.text = $"Current turn: {entity.name} N {currentTurnCount}";
        UIManager.Instance?.ChangeTurn(currentOrderIndex, turnOrder.Count);
        //Every object whose turn needs to be handled should have a EntityTurnHandler's subclass component.
        //entity.GetComponent<EntityTurnHandler>()?.OnTurnStart();
        GlobalEventManager.InvokeStartTurnEvent(entity);
    }
    /// <summary>
    /// Method to get current active kingdom
    /// </summary>
    /// <returns>Returns BaseKingdom whose turn is it now </returns>
    public BaseKingdom GetCurrentActingKingdom()
    {
        return turnOrder[currentOrderIndex];
    }

    public void OnTurnEnd()
    {
        //do something at the end of the turn
        currentOrderIndex++;
        if (currentOrderIndex >= turnOrder.Count)
        {
            currentOrderIndex = 0;
            NextTurn();
        }

        //Debug.Log($"Turn {currentTurnCount} End: {turnOrder[currentOrderIndex].name}'s turn.");

        //when finish the turn, call NextTurn
        OnTurnStart(turnOrder[currentOrderIndex]);
    }

    private void NextTurn()
    {
        currentTurnCount++;
        GlobalEventManager.InvokeEndTurnEvent(turnOrder[currentOrderIndex]);
        Debug.Log("Next Turn");
        //increment turn count
        

        //call OnTurnStart to start the next turn
        //OnTurnStart(turnOrder[currentOrderIndex]);
    }

}
