using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a character's complete inventory system.
/// Manages multiple InventorySlotHolders (e.g., GearSlots, StorageSlots).
/// </summary>
public class Inventory
{
    /// <summary>
    /// List of all slot holders managed by this inventory.
    /// </summary>
    public List<InventorySlotHolder> SlotHolders { get; private set; } = new List<InventorySlotHolder>();

    // /// <summary>
    // /// Maximum number of slots in the general storage inventory.
    // /// kept for initialization params logic
    // /// </summary>
    // public int maxInventorySlots;

    // /// <summary>
    // /// Maximum stack size per storage slot.
    // /// </summary>
    // public int stackPerStorageSlot;

    // Constructors kept compatible in signature but refactored internally

    // public Inventory(int maxSlots, int stackPerSlot) : this(maxSlots, stackPerSlot, null)
    // {
    // }

    // public Inventory(int maxSlots, int stackPerSlot, BaseGridUnitScript ownerEntity)
    // {
    //     // maxInventorySlots = maxSlots;
    //     // stackPerStorageSlot = stackPerSlot;
    //     Initialize(ownerEntity);
    // }

    // public Inventory(int maxSlots, BaseGridUnitScript ownerEntity) : this(maxSlots, 1, ownerEntity) { }

    /// <summary>
    /// Initializes all slot holders.
    /// </summary>
    public Inventory(BaseGridUnitScript ownerEntity, int maxSlots, int stackPerSlot)
    {
        // Initialize GearSlots
        var gearSlots = new GearSlots(ownerEntity);
        SlotHolders.Add(gearSlots);

        // Initialize StorageSlots
        var storageSlots = new StorageSlots(maxSlots, stackPerSlot, ownerEntity);
        SlotHolders.Add(storageSlots);
    }

    /// <summary>
    /// Adds a single item to the inventory (delegates to holders).
    /// </summary>
    public bool AddItem(InventoryItem itemToAdd)
    {
        return AddItem(itemToAdd, 1);
    }

    /// <summary>
    /// Adds items to the first compatible holder.
    /// </summary>
    public bool AddItem(InventoryItem itemToAdd, int amountToAdd)
    {
        foreach (var holder in SlotHolders)
        {
            if (holder.AddItem(itemToAdd, amountToAdd))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Retrieves a specific type of slot holder.
    /// </summary>
    /// <typeparam name="T">The type of holder to retrieve.</typeparam>
    /// <returns>The holder instance, or null if not found.</returns>
    public T GetSlotHolder<T>() where T : InventorySlotHolder
    {
        return SlotHolders.OfType<T>().FirstOrDefault();
    }
}
