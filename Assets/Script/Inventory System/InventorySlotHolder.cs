using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for any component that holds and manages a collection of InventorySlots.
/// Examples: GearSlots (equipment), StorageSlots (backpack/storage).
/// </summary>
public abstract class InventorySlotHolder
{
    /// <summary>
    /// The collection of slots managed by this holder.
    /// </summary>
    protected List<InventorySlot> slots = new List<InventorySlot>();

    /// <summary>
    /// Initializes the holder and its slots with the given owner.
    /// </summary>
    /// <param name="owner">The entity that owns these slots.</param>
    // public abstract InventorySlotHolder(BaseGridUnitScript owner);

    /// <summary>
    /// Attempts to add an item to one of the slots in this holder.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The amount to add.</param>
    /// <returns>True if the item (or part of it if we eventually support partial adds) was added.</returns>
    public abstract bool AddItem(InventoryItem item, int amount);

    /// <summary>
    /// Returns the list of slots managed by this holder.
    /// </summary>
    // public List<InventorySlot> GetSlots()
    // {
    //     return slots;
    // }

}
