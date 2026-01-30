/// <summary>
/// Static utility class for determining item-slot compatibility.
/// Uses a compatibility matrix to validate whether an item type can be placed in a specific slot type.
/// Usage: Call IsItemCompatibleWithSlot() to check compatibility before adding items to slots.
/// </summary>
public static class SlotCompatibility
{
    /// <summary>
    /// 2D compatibility matrix mapping item types to slot types.
    /// Rows: ItemType (General, Helmet, Armor, OneHandedMelee, TwoHandedMelee, Ranged, Trinket, Shield)
    /// Columns: SlotType (General, Helmet, Armor, MainHand, OffHand, Trinket, Shield)
    /// </summary>
    private static readonly bool[,] compatibilityMatrix = new bool[,]
    {
        // General, Helmet, Armor, MainHand, OffHand, Trinket, Shield
        { true,  false,  false,  false,    false,   false,   false }, // General
        { true,  true,   false,  false,    false,   false,   false }, // Helmet
        { true,  false,  true,   false,    false,   false,   false }, // Armor
        { true,  false,  false,  true,     true,    false,   false }, // OneHandedMelee
        { true,  false,  false,  true,     false,   false,   false }, // TwoHandedMelee
        { true,  false,  false,  true,     false,   false,   false }, // Ranged
        { true,  false,  false,  false,    false,   true,    false }, // Trinket
        { true,  false,  false,  false,    true,    false,   true  }  // Shield
    };

    /// <summary>
    /// Checks if an item type can be placed in a specific slot type.
    /// </summary>
    /// <param name="itemType">The type of item to check.</param>
    /// <param name="slotType">The type of slot to check against.</param>
    /// <returns>True if the item can be placed in the slot, false otherwise.</returns>
    public static bool IsItemCompatibleWithSlot(ItemType itemType, SlotType slotType)
    {
        return compatibilityMatrix[(int)itemType, (int)slotType];
    }
}