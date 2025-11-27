using UnityEngine;

/// <summary>
/// Handles turn-based logic for the CityManager
/// This component should be attached to the same GameObject as CityManager
/// </summary>
public class KingdomTurnHandler : EntityTurnHandler
{

    /// <summary>
    /// Called when this entity's turn starts
    /// Progresses all city resource accumulation by one turn
    /// </summary>
    //public override void OnTurnStart()
    //{
    //    base.OnTurnStart();
    //    GetComponent<PlayerKingdom>().StartTurn();
    //}
}

