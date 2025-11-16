using UnityEngine;

// Ai controlled kingdom class
public class AIKingdom : BaseKingdom
{
    int relationsWithPlayer;
    int madnessLevel;
    public bool IsBuildUnit { get;private set; }
    
    public void StartTurn()
    {
        Debug.Log($"AI Kingdom StartTurn | {gameObject.name}");
        if (AIController.Instance.ExecuteTurn(this))
        {
            EndTurn();   
        }
    }

    public void EndTurn()
    {
        Debug.Log($"AI Kingdom EndTurn | {gameObject.name}");
        IsBuildUnit = false;
        TurnManager.instance.OnTurnEnd();
    }
}