using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single inventory slot that can contain a stack of items.
/// Handles item storage, stacking, compatibility checking, and transfers.
/// Usage: Created programmatically by Inventory and StorageInventory classes.
/// </summary>
public class InventorySlot
{
    /// <summary>The type of slot, determining what items can be placed here.</summary>
    public SlotType slotType;
    
    /// <summary>The item currently stored in this slot, or null if empty.</summary>
    public InventoryItem item;
    
    /// <summary>The quantity of items in this slot.</summary>
    public int amount;
    
    /// <summary>Maximum number of items that can be stacked in this slot.</summary>
    public int maxStackSize;

    /// <summary>
    /// Creates a general-purpose inventory slot with default slot type.
    /// </summary>
    /// <param name="maxStackSize">Maximum items that can stack in this slot.</param>
    public InventorySlot(int maxStackSize)
    {
        this.maxStackSize = maxStackSize;
        item = null;
        amount = 0;
        slotType = SlotType.General;
    }

    /// <summary>
    /// Creates a specialized inventory slot for specific equipment types.
    /// </summary>
    /// <param name="slotType">The type of items this slot accepts (Helmet, Armor, etc.).</param>
    /// <param name=\"maxStackSize\">Maximum items that can stack in this slot.</param>
    public InventorySlot(SlotType slotType, int maxStackSize)
    {
        this.maxStackSize = maxStackSize;
        item = null;
        amount = 0;
        this.slotType = slotType;
    }

    /// <summary>
    /// Adds a single item to the slot.
    /// </summary>
    /// <param name=\"newItem\">The item to add.</param>
    /// <returns>True if successfully added, false if incompatible or full.</returns>
    public bool Add(InventoryItem newItem)
    {
        return Add(newItem, 1);
    }

    /// <summary>
    /// Attempts to add items to this slot. Checks compatibility and stacking limits.
    /// Will only add if: item matches existing item (or slot is empty), slot type is compatible,
    /// and there's enough space within stack limits.
    /// </summary>
    /// <param name=\"newItem\">The item to add.</param>
    /// <param name=\"amountToAdd\">Number of items to add.</param>
    /// <returns>True if items were successfully added, false otherwise.</returns>
    public bool Add(InventoryItem newItem, int amountToAdd)
    {
        if(!SlotCompatibility.IsItemCompatibleWithSlot(newItem.itemType, slotType))
        {
            Debug.Log("Unable to Add Item. Item type is not compatible with slot type.");
            return false;
        }
        if (item != null)
        {
            int newAmount = amount + amountToAdd;
            if (item == newItem && newAmount <= maxStackSize && newAmount <= newItem.maxStackSize)
            {
                amount = newAmount;
                return true;
            }
            else
            {
                Debug.Log("Unable to Add Item. Slot already contains a different item or is full.");
                return false;
            }
        }
        else if(amountToAdd <= maxStackSize && amountToAdd <= newItem.maxStackSize)
        {
            item = newItem;
            amount = amountToAdd;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Clears the slot, removing all items.
    /// </summary>
    /// <returns>Always returns true.</returns>
    public bool Remove()
    {
        item = null;
        amount = 0;
        return true;
    }

    /// <summary>
    /// Transfers all items from this slot to another slot.
    /// Checks compatibility before transferring.
    /// </summary>
    /// <param name=\"targetSlot\">The destination slot.</param>
    /// <returns>True if transfer successful, false if incompatible or target is full.</returns>
    public bool TransferTo(InventorySlot targetSlot)
    {
        if (item == null || targetSlot == null)
        {
            return false;
        }
        if (!SlotCompatibility.IsItemCompatibleWithSlot(item.itemType, targetSlot.slotType))
        {
            Debug.Log("Cannot transfer item: incompatible slot type.");
            return false;
        }
        if(targetSlot.Add(item, amount))
        {
            Remove();
            return true;
        }
        return false;
    }
}
