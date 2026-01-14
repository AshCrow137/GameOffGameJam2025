using UnityEngine;

/// <summary>
/// Singleton that manages the visual representation of items being dragged between inventory slots.
/// Follows the mouse cursor while dragging and handles drop logic.
/// Usage: Automatically managed by InventoryItemDragger, should exist as a single instance in the scene.
/// </summary>
public class InventoryItemDragged : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    
    /// <summary>Singleton instance of the dragged item manager.</summary>
    public static InventoryItemDragged Instance { get; private set; }
    
    /// <summary>The UI slot being visually dragged.</summary>
    public InventorySlotUI draggedSlot;
    
    private RectTransform draggedSlotTransform;
    private InventorySlotUI originalSlot;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            draggedSlotTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts dragging an item from the specified slot.
    /// </summary>
    /// <param name="slot">The slot to take the item from.</param>
    public void TakeFrom(InventorySlotUI slot)
    {
        if (draggedSlot == null)
        {
            draggedSlot = GetComponent<InventorySlotUI>();
        }
        originalSlot = slot;
        if(slot.slot == null || slot.slot.item == null)
        {
            return;
        }
        draggedSlot.Assign(slot.slot);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Updates the dragged item's position to follow the mouse cursor.
    /// </summary>
    private void Update()
    {
        if (draggedSlot.slot != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint);
            draggedSlotTransform.localPosition = localPoint;
        }
    }

    /// <summary>
    /// Attempts to drop the dragged item into a target slot.
    /// </summary>
    /// <param name="targetSlotUI">The slot to drop the item into.</param>
    /// <returns>True if the item was successfully transferred, false otherwise.</returns>
    public bool DropAt(InventorySlotUI targetSlotUI)
    {
        if (draggedSlot == null || draggedSlot.slot == null || targetSlotUI == null)
        {
            return CancelDrag();
        }

        if(originalSlot.TransferTo(targetSlotUI))
        {
            CancelDrag();
            return true;
        }
        return CancelDrag();
    }

    /// <summary>
    /// Cancels the current drag operation and cleans up.
    /// </summary>
    /// <returns>Always returns false.</returns>
    private bool CancelDrag()
    {
        if (draggedSlot != null)
        {
            draggedSlot.Clear();
        }
        gameObject.SetActive(false);
        return false;
    }
}