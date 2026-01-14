using UnityEngine; 
using UnityEngine.UI;

/// <summary>
/// Represents a character's complete inventory system with equipment slots and storage.
/// Manages helmet, armor, weapons, trinket, and general storage.
/// Usage: Create via constructor with desired slot capacity and stack size.
/// </summary>
public class Inventory
{
    /// <summary>Equipment slot for helmets.</summary>
    public InventorySlot helmet;
    
    /// <summary>Equipment slot for body armor.</summary>
    public InventorySlot armour;
    
    /// <summary>Equipment slot for main hand weapons/items.</summary>
    public InventorySlot mainHand;
    
    /// <summary>Equipment slot for off-hand items (shields, secondary weapons).</summary>
    public InventorySlot offHand;
    
    /// <summary>Equipment slot for trinkets and accessories.</summary>
    public InventorySlot trinket;
    
    /// <summary>General storage inventory for non-equipped items.</summary>
    public StorageInventory playerStorage;
    
    /// <summary>Maximum number of slots in the inventory.</summary>
    public int maxInventorySlots;
    
    /// <summary>Maximum stack size per storage slot.</summary>
    public int stackPerStorageSlot;

    /// <summary>
    /// Initializes a new inventory with specified capacity.
    /// </summary>
    /// <param name="maxSlots">Maximum number of storage slots.</param>
    /// <param name="stackPerSlot">Maximum items per stack in storage.</param>
    public Inventory(int maxSlots, int stackPerSlot)
    {
        maxInventorySlots = maxSlots;
        stackPerStorageSlot = stackPerSlot;
        Initialize();
    }

    /// <summary>
    /// Initializes all equipment slots and storage inventory.
    /// </summary>
    public void Initialize()
    {
        helmet = new InventorySlot(SlotType.Helmet, 1);
        armour = new InventorySlot(SlotType.Armor, 1);
        mainHand = new InventorySlot(SlotType.MainHand, 1);
        offHand = new InventorySlot(SlotType.OffHand, 1);
        trinket = new InventorySlot(SlotType.Trinket, 1);
        playerStorage = new StorageInventory(maxInventorySlots, 99);
    }

    /// <summary>
    /// Adds a single item to the inventory.
    /// </summary>
    /// <param name="itemToAdd">The item to add.</param>
    /// <returns>True if item was successfully added, false otherwise.</returns>
    public bool AddItem(InventoryItem itemToAdd)
    {
        return AddItem(itemToAdd, 1);
    }

    /// <summary>
    /// Adds items to the first compatible slot (equipment or storage).
    /// Tries equipment slots first, then general storage.
    /// </summary>
    /// <param name="itemToAdd">The item to add.</param>
    /// <param name="amountToAdd">Number of items to add.</param>
    /// <returns>True if items were successfully added, false if no space available.</returns>
    public bool AddItem(InventoryItem itemToAdd, int amountToAdd)
    {
        if (helmet.Add(itemToAdd, amountToAdd)) return true;
        if (armour.Add(itemToAdd, amountToAdd)) return true;
        if (mainHand.Add(itemToAdd, amountToAdd)) return true;
        if (offHand.Add(itemToAdd, amountToAdd)) return true;
        if (trinket.Add(itemToAdd, amountToAdd)) return true;
        return playerStorage.Add(itemToAdd, amountToAdd);
    }
}