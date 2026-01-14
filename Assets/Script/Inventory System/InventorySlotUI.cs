using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Manages the visual representation of an inventory slot in the UI.
/// Displays item icon, amount text, and handles slot assignment/clearing.
/// Usage: Attach to inventory slot UI GameObjects in the scene. Works with InventoryItemDragger for drag/drop.
/// </summary>
public class InventorySlotUI : MonoBehaviour
{
    /// <summary>Text display showing the item quantity in this slot.</summary>
    public TextMeshProUGUI amountText;
    
    /// <summary>Image component displaying the item's icon.</summary>
    public Image slotImage;
    
    /// <summary>The inventory slot data this UI represents.</summary>
    public InventorySlot slot;

    /// <summary>
    /// Updates the visual display based on the current slot's item and amount.
    /// Shows icon and quantity if item exists, otherwise hides display.
    /// </summary>
    private void UpdateUI()
    {
        if (slot != null && slot.item != null)
        {
            slotImage.sprite = slot.item.itemIcon;
            amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
            slotImage.enabled = true;
        }
        else
        {
            slotImage.sprite = null;
            amountText.text = "";
            slotImage.enabled = false;
        }
    }

    /// <summary>
    /// Transfers items from this slot to another slot UI.
    /// Updates both UIs after successful transfer.
    /// </summary>
    /// <param name=\"newSlot\">The target slot UI to transfer items to.</param>
    /// <returns>True if transfer succeeded, false if slot is null or transfer failed.</returns>
    public bool TransferTo(InventorySlotUI newSlot)
    {
        if (newSlot == null || slot == null)
        {
            return false;
        }
        if (slot.TransferTo(newSlot.slot))
        {
            UpdateUI();
            newSlot.UpdateUI();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Assigns a new inventory slot to this UI element and refreshes the display.
    /// </summary>
    /// <param name=\"newSlot\">The inventory slot to assign.</param>
    public void Assign(InventorySlot newSlot)
    {
        slot = newSlot;
        UpdateUI();
    }

    /// <summary>
    /// Clears the slot assignment and updates the display to empty.
    /// </summary>
    public void Clear()
    {
        slot = null;
        UpdateUI();
    }
}