using UnityEngine;

/// <summary>
/// Handles turn-based logic for the BuildingManager
/// This component should be attached to the same GameObject as BuildingManager
/// </summary>
public class BuildingTurnHandler : EntityTurnHandler
{
    // private BuildingManager buildingManager;

    // private void Awake()
    // {
    //     // Get reference to BuildingManager on the same GameObject
    //     buildingManager = GetComponent<BuildingManager>();
        
    //     if (buildingManager == null)
    //     {
    //         Debug.LogError("BuildingTurnHandler requires a BuildingManager component on the same GameObject!");
    //     }
    // }

    /// <summary>
    /// Called when this entity's turn starts
    /// Progresses all building constructions by one turn
    /// </summary>
    public new void OnTurnStart()
    {
        base.OnTurnStart(); // Call base implementation for logging
        Debug.Log("BuildingTurnHandler: OnTurnStart called.");
        GetComponent<BuildingManager>().StartTurn();
        
        // if (buildingManager != null)
        // {
        //     buildingManager.StartTurn();
        // }
    }
}

