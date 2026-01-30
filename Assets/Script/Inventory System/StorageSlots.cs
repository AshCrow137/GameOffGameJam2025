using UnityEngine;

/// <summary>
/// Manages a collection of general-purpose storage slots.
/// Replaces the old StorageInventory class.
/// </summary>
public class StorageSlots : InventorySlotHolder
{
    public int maxSlots { get; private set; }


    public StorageSlots(int maxSlots, int maxStackPerSlot)
    {
        this.maxSlots = maxSlots;

        for (int i = 0; i < maxSlots; i++)
        {
            // Create a general slot. Owner might be needed for some slot logic, passing it in.
            AddSlot(new InventorySlot(SlotType.General, maxStackPerSlot));
        }
    }

    public override bool AddItem(InventoryItem item, int amount)
    {
        // Try to stack with existing items first
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.Add(item, amount))
            {
                return true;
            }
        }

        // Try to find an empty slot
        foreach (var slot in slots)
        {
            if (slot.item == null && slot.Add(item, amount))
            {
                return true;
            }
        }

        return false;
    }

    public InventorySlot GetSlot(int index)
    {
        return slots[index];
    }
}
