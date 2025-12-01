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

    public List<BaseKingdom> GetActingKingdoms()
    {
        return turnOrder;
    }
    //public TurnManager(List<GameObject> turnOrder)
    //{
    //    this.turnOrder = turnOrder;
    //}
    public List<BaseKingdom> GetKingdoms ()=> turnOrder;
    public void Initialize()
    {
        instance = this;
        currentOrderIndex = 0;
        StartTurn(turnOrder[currentOrderIndex]);
       
    }
    public void RemoveKingdomFromTurnOrder(BaseKingdom kingdom)
    {
        int index = turnOrder.IndexOf(kingdom);
        if (index != -1)
        {
            turnOrder.RemoveAt(index);
        }
    }

    public void StartTurn(BaseKingdom entity)
    {
        //camera focus on the current Player/Unit
        //Debug.Log($"Turn {currentTurnCount} Start: {entity.name}'s turn.");
        turnText.text = $"Current turn: {entity.name} N {currentTurnCount}";
        UIManager.Instance?.ChangeTurn(currentTurnCount);
        //Every object whose turn needs to be handled should have a EntityTurnHandler's subclass component.
        //entity.GetComponent<EntityTurnHandler>()?.StartTurn();
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
    public void EndPlayerTurn()
    {
        if(turnOrder[currentOrderIndex].GetType() == typeof(PlayerKingdom))
        {
            OnTurnEnd();
        }
    }

    public void OnTurnEnd()
    {
        //do something at the end of the turn
        if (turnOrder[currentOrderIndex].GetType() == typeof(PlayerKingdom))
        {
            AudioManager.Instance.ui_gameplay_startTurn.Post(gameObject);
        }
        currentOrderIndex++;
        if (currentOrderIndex >= turnOrder.Count)
        {
            currentOrderIndex = 0;
            NextRound();
        }

        //Debug.Log($"Turn {currentTurnCount} End: {turnOrder[currentOrderIndex].name}'s turn.");
        GlobalEventManager.InvokeEndTurnEvent(turnOrder[currentOrderIndex]);
        //when finish the turn, call NextRound
        StartTurn(turnOrder[currentOrderIndex]);

       
    }

    private void NextRound()
    {
        currentTurnCount++;

        Debug.Log("Next Turn");
        //increment turn count
        

        //call StartTurn to start the next turn
        //StartTurn(turnOrder[currentOrderIndex]);
    }

}
