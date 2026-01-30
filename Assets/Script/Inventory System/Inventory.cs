using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

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
    public UnityEvent<InventoryItem, int, int, Type> OnItemAdded = new();
    public UnityEvent<InventoryItem, int, int, Type> OnItemRemoved = new();


    /// <summary>
    /// Initializes all slot holders.
    /// </summary>
    public Inventory(int maxSlots, int stackPerSlot)
    {
        AddSlotHolder(new GearSlots());
        AddSlotHolder(new StorageSlots(maxSlots, stackPerSlot));
    }

    private void AddSlotHolder(InventorySlotHolder holder)
    {
        SlotHolders.Add(holder);
        holder.OnItemAdded.AddListener((item, amount, index) => OnItemAdded?.Invoke(item, amount, index, holder.GetType()));
        holder.OnItemRemoved.AddListener((item, amount, index) => OnItemRemoved?.Invoke(item, amount, index, holder.GetType()));
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
