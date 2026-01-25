using UnityEngine;

/// <summary>
/// Base class for handling entity turns
/// </summary>
public class EntityTurnHandler : MonoBehaviour
{
    public virtual void OnTurnStart()
    {
        Debug.Log($"Handling {gameObject.name}'s turn start.");

    }
}