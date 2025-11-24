using UnityEngine;

public class BotTurnHandler : EntityTurnHandler
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GetComponent<AIKingdom>().StartTurn();
    }
    
    
}