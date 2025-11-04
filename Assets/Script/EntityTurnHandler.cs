using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for handling entity turns
/// </summary>
public class EntityTurnHandler : MonoBehaviour
{
    public void OnTurnStart()
    {
        Debug.Log($"Handling {gameObject.name}'s turn start.");

    }
}