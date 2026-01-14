using UnityEngine;

/// <summary>
/// Manages a collection of general storage slots for non-equipped items.
/// Handles adding items to available slots, stacking, and removal.
/// Usage: Created by the Inventory class to provide general item storage.
/// </summary>
public class StorageInventory
{
    /// <summary>Array of all storage slots.</summary>
    public InventorySlot[] storageSlots;
    
    /// <summary>Maximum number of slots available for storage.</summary>
    public int maxStorageSlots;
    
    /// <summary>Maximum stack size for items in storage slots.</summary>
    public int maxStackPerSlot;

    /// <summary>
    /// Initializes a new storage inventory with the specified capacity.
    /// </summary>
    /// <param name="size">Number of storage slots to create.</param>
    /// <param name="maxStackPerSlot">Maximum items per stack in each slot.</param>
    public StorageInventory(int size, int maxStackPerSlot)
    {
        this.maxStackPerSlot = maxStackPerSlot;
        maxStorageSlots = size;
        InitializeStorageInventory();
    }

    /// <summary>
    /// Creates all storage slots with empty initial state.
    /// </summary>
    private void InitializeStorageInventory()
    {
        storageSlots = new InventorySlot[maxStorageSlots];
        for (int i = 0; i < maxStorageSlots; i++)
        {
            storageSlots[i] = new InventorySlot(maxStackPerSlot);
        }
    }

    /// <summary>
    /// Adds a single item to storage.
    /// </summary>
    /// <param name="itemToAdd">The item to add.</param>
    /// <returns>True if successfully added, false if storage is full.</returns>
    public bool Add(InventoryItem itemToAdd)
    {
        return Add(itemToAdd, 1);
    }

    /// <summary>
    /// Adds items to storage. First attempts to stack with existing items,
    /// then uses empty slots if no matching stack is found.
    /// </summary>
    /// <param name="itemToAdd">The item to add.</param>
    /// <param name="amountToAdd">Number of items to add.</param>
    /// <returns>True if items were added, false if storage is full or items couldn't be added.</returns>
    public bool Add(InventoryItem itemToAdd, int amountToAdd)
    {
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (storageSlots[i].item != null && storageSlots[i].Add(itemToAdd, amountToAdd))
            {
                return true;
            }
        }
        for(int i = 0; i < storageSlots.Length; i++)
        {
            if (storageSlots[i].item == null)
            {
                if (storageSlots[i].Add(itemToAdd, amountToAdd))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all items from the specified slot.
    /// </summary>
    /// <param name="slot">The slot to clear.</param>
    /// <returns>True if slot was found and cleared, false otherwise.</returns>
    public bool RemoveAtSlot(InventorySlot slot)
    {
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (storageSlots[i] == slot)
            {
                return storageSlots[i].Remove();
            }
        }
        return false;
    }

    /// <summary>
    /// Removes all items from the slot at the specified index.
    /// </summary>
    /// <param name="index">The index of the slot to clear.</param>
    /// <returns>True if index is valid and slot was cleared, false otherwise.</returns>
    public bool RemoveAtSlot(int index)
    {
        if(index >= 0 && index < storageSlots.Length)
        {
            return storageSlots[index].Remove();
        }
        return false;
    }

    /// <summary>
    /// Finds and removes the first occurrence of the specified item.
    /// </summary>
    /// <param name="itemToRemove">The item to remove.</param>
    /// <returns>True if item was found and removed, false if item not found.</returns>
    public bool RemoveItem(InventoryItem itemToRemove)
    {
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (storageSlots[i].item == itemToRemove)
            {
                return storageSlots[i].Remove();
            }
        }
        return false;
    }
}