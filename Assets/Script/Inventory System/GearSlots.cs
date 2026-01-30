using UnityEngine;
using System.Linq;

/// <summary>
/// Manages specific equipment slots for a character.
/// </summary>
public class GearSlots : InventorySlotHolder
{
    private const int EQUIP_STACK_SIZE = 1;

    public GearSlots()
    {
        AddSlot(new InventorySlot(SlotType.Helmet, EQUIP_STACK_SIZE));
        AddSlot(new InventorySlot(SlotType.Armor, EQUIP_STACK_SIZE));
        AddSlot(new InventorySlot(SlotType.MainHand, EQUIP_STACK_SIZE));
        AddSlot(new InventorySlot(SlotType.OffHand, EQUIP_STACK_SIZE));
        AddSlot(new InventorySlot(SlotType.Trinket, EQUIP_STACK_SIZE));
    }

    public override bool AddItem(InventoryItem item, int amount)
    {
        // Try all slots - they check their own compatibility
        foreach (var slot in slots)
        {
            if (slot.Add(item, amount)) return true;
        }

        return false;
    }

    public InventorySlot GetSlot(SlotType type)
    {
        return slots.FirstOrDefault(s => s.slotType == type);
    }
}

