using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the opening and closing of inventory UIs for different characters.
/// Handles positioning of inventory windows and tracks open instances.
/// Usage: Attach to a manager GameObject (e.g., UIManager). Access via singleton instance.
/// </summary>
public class InventoryUIToggle : MonoBehaviour
{
    public static InventoryUIToggle instance;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    public void Instantiate()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private InventoryUI InventoryUIPrefab;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private UIManager uiManager;

    private HashSet<InventoryUI> openInventories = new HashSet<InventoryUI>();

    /// <summary>
    /// Opens an inventory UI for the specified unit at the given mouse position.
    /// Closes any existing open inventory if needed (though implementation supports multiple via tracking).
    /// </summary>
    /// <param name="unit">The unit whose inventory to open.</param>
    /// <param name="mousePosition">The screen position to spawn the inventory UI near.</param>
    public void Open(BaseGridUnitScript unit, Vector3 mousePosition)
    {
        Vector3 inventoryOpenPosition = GetInventoryOpenPosition(mousePosition);
        InventoryUI inventoryUI = PoolingEntity.Spawn(InventoryUIPrefab, inventoryOpenPosition, Quaternion.identity, inventoryParent);
        if (inventoryUI.closeButton != null)
        {
            inventoryUI.closeButton.onClick.RemoveAllListeners();
            inventoryUI.closeButton.onClick.AddListener(() => Close(inventoryUI));
        }
        else
        {
            Debug.LogError("InventoryUI close button not assigned in the Inspector");
        }

        if (inventoryUI.eventTrigger != null)
        {
            inventoryUI.eventTrigger.triggers.Clear();

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { uiManager.OnMouseEnterCanvasElement(); });
            inventoryUI.eventTrigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { uiManager.OnMouseExitCanvasElement(); });
            inventoryUI.eventTrigger.triggers.Add(exitEntry);
        }
        else
        {
            Debug.LogError("InventoryUI eventTrigger not assigned in the Inspector");
        }

        inventoryUI.Assign(unit);
        inventoryUI.inventoryDragger.canvas = inventoryCanvas;
        openInventories.Add(inventoryUI);
    }

    /// <summary>
    /// Closes the specified inventory UI.
    /// </summary>
    /// <param name="inventoryUI">The inventory UI instance to close.</param>
    public void Close(InventoryUI inventoryUI)
    {
        if (inventoryUI != null)
        {
            if (inventoryUI.eventTrigger != null)
            {
                inventoryUI.eventTrigger.triggers.Clear();
            }

            openInventories.Remove(inventoryUI);
            if (inventoryUI.TryGetComponent(out PoolingEntity poolingEntity))
            {
                poolingEntity.Despawn();
            }
        }
    }

    /// <summary>
    /// Closes all currently open inventory UIs.
    /// </summary>
    public void CloseAll()
    {
        if (openInventories.Count == 0) { return; }
        List<InventoryUI> toClose = new List<InventoryUI>(openInventories);
        foreach (var inventoryUI in toClose)
        {
            Close(inventoryUI);
        }
        openInventories.Clear();
    }

    /// <summary>
    /// Calculates a valid screen position for the inventory UI to ensure it stays within screen bounds.
    /// </summary>
    /// <param name="mousePosition">The initial desired position (usually mouse cursor).</param>
    /// <returns>A corrected position vector that keeps the UI on screen.</returns>
    private Vector3 GetInventoryOpenPosition(Vector3 mousePosition)
    {
        RectTransform rectTransform = InventoryUIPrefab.GetComponent<RectTransform>();
        float scaleFactor = inventoryCanvas.scaleFactor;
        float width = rectTransform.rect.width * scaleFactor;
        float height = rectTransform.rect.height * scaleFactor;

        Vector2 pivot = rectTransform.pivot;
        Vector3 result = mousePosition;

        // Check Right
        float currentRight = result.x + (width * (1f - pivot.x));
        if (currentRight > Screen.width)
        {
            result.x -= (currentRight - Screen.width);
        }

        // Check Left (after potential right shift)
        float currentLeft = result.x - (width * pivot.x);
        if (currentLeft < 0)
        {
            result.x += (0 - currentLeft);
        }

        // Check Top
        float currentTop = result.y + (height * (1f - pivot.y));
        if (currentTop > Screen.height)
        {
            result.y -= (currentTop - Screen.height);
        }

        // Check Bottom
        float currentBottom = result.y - (height * pivot.y);
        if (currentBottom < 0)
        {
            result.y += (0 - currentBottom);
        }

        return result;
    }

}

