using UnityEngine;

/// <summary>
/// Scriptable Object representing an inventory item with its properties.
/// Usage: Create via Assets > Create > Inventory > Item menu.
/// Configure item type, name, icon, description, and stack size in the inspector.
/// </summary>
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    /// <summary>The category/type of this item (e.g., Helmet, Weapon, Consumable).</summary>
    public ItemType itemType;
    
    /// <summary>The display name of the item.</summary>
    public string itemName;
    
    /// <summary>The sprite icon representing this item in the UI.</summary>
    public Sprite itemIcon;
    
    /// <summary>Descriptive text explaining the item's properties or effects.</summary>
    public string description;
    
    /// <summary>Maximum number of this item that can stack in a single slot.</summary>
    public int maxStackSize;
}