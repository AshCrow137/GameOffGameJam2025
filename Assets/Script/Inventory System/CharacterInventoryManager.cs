using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the inventory for a character unit in the game.
/// Handles initialization of player inventory and test item setup.
/// Usage: Attach to character GameObjects that need inventory management.
/// </summary>
[RequireComponent(typeof(BaseGridUnitScript))]
public class CharacterInventoryManager : MonoBehaviour
{
    /// <summary>Reference to the player's inventory containing equipment slots and storage.</summary>
    public Inventory playerInventory;

    public void Initialize(int storageSlots)
    {
        playerInventory = new Inventory(storageSlots, 1, GetComponent<BaseGridUnitScript>());
    }

    /// <summary>
    /// Adds an item directly to the inventory.
    /// Intended for automation or debugging purposes where UI interaction is bypassed.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(InventoryItem item)
    {
        playerInventory.AddItem(item);
    }



    [Header("Test Configuration")]
    [SerializeField] private List<InventoryItem> items;

    /// <summary>
    /// Initializes the player inventory with default capacity and adds test items.
    /// </summary>
    public void Initialize()
    {
        BaseGridUnitScript unit = GetComponent<BaseGridUnitScript>();
        playerInventory = new Inventory(6, 1, unit);
        foreach (InventoryItem item in items)
        {
            playerInventory.AddItem(item);
        }
    }


}