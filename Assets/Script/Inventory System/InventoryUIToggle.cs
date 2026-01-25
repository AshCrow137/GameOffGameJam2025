using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryUIToggle : MonoBehaviour
{
    // remove awake call later
    //private void Awake()
    //{
    //    Instantiate();
    //}
    public static InventoryUIToggle instance;
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

