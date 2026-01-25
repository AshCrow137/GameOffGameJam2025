using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the inventory for a character unit in the game.
/// Handles initialization of player inventory and test item setup.
/// Usage: Attach to character GameObjects that need inventory management.
/// </summary>
public class CharacterInventoryManager : MonoBehaviour
{
    /// <summary>Reference to the player's inventory containing equipment slots and storage.</summary>
    public Inventory playerInventory;
    
    /// <summary>Reference to the inventory UI for displaying and interacting with items.</summary>
    public InventoryUI inventoryUI;



    ///////////// / Test Items /////////////
    [SerializeField] private List<InventoryItem> items;

    /// <summary>
    /// Initializes the player inventory with default capacity and adds test items.
    /// </summary>
    private void Start()
    {
        BaseGridUnitScript unit = GetComponent<BaseGridUnitScript>();
        playerInventory = new Inventory(6, 1, unit);
        foreach (InventoryItem item in items)
        {
            playerInventory.AddItem(item);
        }
    }
    ////////////////////////////////////////

}