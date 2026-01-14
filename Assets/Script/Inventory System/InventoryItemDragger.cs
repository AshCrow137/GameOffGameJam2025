using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles drag and drop input events for inventory items.
/// Detects pointer down/up events and communicates with InventoryItemDragged for visual feedback.
/// Usage: Attach to inventory slot UI GameObjects alongside InventorySlotUI.
/// </summary>
public class InventoryItemDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>Reference to the current inventory slot UI.</summary>
    public InventorySlotUI currentSlot;

    /// <summary>
    /// Called when the pointer is pressed down on the slot. Initiates drag operation.
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentSlot == null)
        {
            currentSlot = GetComponent<InventorySlotUI>();
        }
        InventoryItemDragged.Instance.TakeFrom(currentSlot);
    }

    /// <summary>
    /// Called when the pointer is released. Attempts to drop the item at the current position.
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject droppedOn = eventData.pointerCurrentRaycast.gameObject;
        InventoryItemDragged.Instance.DropAt(GetSlotAtPosition(eventData));
    }

    /// <summary>
    /// Finds the inventory slot UI at the pointer's position using raycasting.
    /// </summary>
    /// <param name="eventData">Current event data containing pointer position.</param>
    /// <returns>The InventorySlotUI at the position, or null if none found.</returns>
    private InventorySlotUI GetSlotAtPosition(PointerEventData eventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            InventorySlotUI slotUI = result.gameObject.GetComponent<InventorySlotUI>();
            if (slotUI != null && slotUI != InventoryItemDragged.Instance.draggedSlot)
            {
                return slotUI;
            }
        }

        return null;
    }
}