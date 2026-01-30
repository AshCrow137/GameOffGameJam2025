using UnityEngine;
using System.Linq;

/// <summary>
/// Manages specific equipment slots for a character.
/// </summary>
public class GearSlots : InventorySlotHolder
{
    private const int EQUIP_STACK_SIZE = 1;

    public GearSlots(BaseGridUnitScript owner)
    {
        slots.Add(new EquipmentInventorySlot(SlotType.Helmet, EQUIP_STACK_SIZE, owner));
        slots.Add(new EquipmentInventorySlot(SlotType.Armor, EQUIP_STACK_SIZE, owner));
        slots.Add(new EquipmentInventorySlot(SlotType.MainHand, EQUIP_STACK_SIZE, owner));
        slots.Add(new EquipmentInventorySlot(SlotType.OffHand, EQUIP_STACK_SIZE, owner));
        slots.Add(new EquipmentInventorySlot(SlotType.Trinket, EQUIP_STACK_SIZE, owner));
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

    public EquipmentInventorySlot GetSlot(SlotType type)
    {
        return slots.FirstOrDefault(s => s.slotType == type) as EquipmentInventorySlot;
    }
}

