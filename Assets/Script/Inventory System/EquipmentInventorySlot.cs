public class EquipmentInventorySlot : InventorySlot
{

    public EquipmentInventorySlot(SlotType slotType, int maxStackSize, BaseGridUnitScript ownerEntity) : base(slotType, maxStackSize, ownerEntity) { }
    public override void OnItemAdded(InventoryItem item, int amount)
    {
        if (item is EquippableInventoryItem equippableItem)
        {
            equippableItem.OnEquip(ownerEntity);
        }
    }

    public override void OnItemRemoved(InventoryItem item, int amount)
    {
        if (item is EquippableInventoryItem equippableItem)
        {
            equippableItem.OnUnequip(ownerEntity);
        }
    }
}