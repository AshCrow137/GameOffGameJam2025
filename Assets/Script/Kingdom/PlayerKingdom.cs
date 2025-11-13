using UnityEngine;

// Player controlled kingdom class
public class PlayerKingdom : BaseKingdom
{

    public void StartTurn()
    {
        KingdomUI.Instance?.StartTurn();
    }
   
}