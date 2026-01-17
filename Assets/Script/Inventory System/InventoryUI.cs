using UnityEngine;

/// <summary>
/// Main UI controller for the inventory system.
/// Manages equipment slots (head, body, hands, etc.) and storage panel.
/// Usage: Attach to the main inventory panel GameObject. Singleton pattern ensures one instance.
/// Requires references to equipment slot UIs and storage panel to be set in inspector.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    /// <summary>Prefab used to instantiate storage slot UI elements.</summary>
    public InventorySlotUI InventorySlotUIPrefab;

    /// <summary>UI element for the head/helmet equipment slot.</summary>
    public InventorySlotUI headSlotUi;
    
    /// <summary>UI element for the body/armor equipment slot.</summary>
    public InventorySlotUI bodySlotUi;
    
    /// <summary>UI element for the main hand equipment slot.</summary>
    public InventorySlotUI handSlotUi;
    
    /// <summary>UI element for the off-hand equipment slot.</summary>
    public InventorySlotUI offhandSlotUi;
    
    /// <summary>UI element for the trinket equipment slot.</summary>
    public InventorySlotUI trinketSlotUi;

    /// <summary>Array of storage slot UI elements, dynamically created.</summary>
    public InventorySlotUI[] slotUis;
    
    /// <summary>Container for storage slot UI elements.</summary>
    public RectTransform storagePanel;
    
    /// <summary>Root GameObject for the entire inventory panel.</summary>
    public GameObject inventoryPanel;
    
    /// <summary>Number of storage slots to create.</summary>
    public int inventorySize = 20;

    /// <summary>Reference to the character whose inventory is currently displayed.</summary>
    public CharacterInventoryManager assignedCharacter;

    /// <summary>Singleton instance for global access.</summary>
    public static InventoryUI Instance;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inventoryPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes storage slot UI elements on start.
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Toggles the visibility of the inventory panel.
    /// </summary>
    public void OnInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    /// <summary>
    /// Assigns a unit's inventory to this UI for display.
    /// </summary>
    /// <param name="unit">The unit whose inventory should be displayed.</param>
    public void Assign(BaseGridUnitScript unit)
    {
        if(unit == null)
        {
            Debug.LogError("InventoryUI Assign called with null unit!");
            return;
        }
        assignedCharacter = unit.GetComponent<CharacterInventoryManager>();
        UpdateInventory(assignedCharacter.playerInventory);
    }

    /// <summary>
    /// Creates storage slot UI elements based on inventory size.
    /// </summary>
    public void Initialize()
    {
        slotUis = new InventorySlotUI[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            InventorySlotUI slotUi = Instantiate(InventorySlotUIPrefab, storagePanel);
            slotUis[i] = slotUi;
        }
    }

    /// <summary>
    /// Updates all UI elements to reflect the current state of the inventory.
    /// Assigns equipment slots and storage slots to their corresponding UI elements.
    /// </summary>
    /// <param name="playerInventory">The inventory to display.</param>
    public void UpdateInventory(Inventory playerInventory)
    {
        headSlotUi.Assign(playerInventory.helmet);
        bodySlotUi.Assign(playerInventory.armour);
        handSlotUi.Assign(playerInventory.mainHand);
        offhandSlotUi.Assign(playerInventory.offHand);
        trinketSlotUi.Assign(playerInventory.trinket);
        for (int i = 0; i < slotUis.Length; i++)
        {
            if (i < playerInventory.playerStorage.maxStorageSlots)
            {
                slotUis[i].Assign(playerInventory.playerStorage.storageSlots[i]);
            }
            else
            {
                slotUis[i].Clear();
            }
        }
    }
}