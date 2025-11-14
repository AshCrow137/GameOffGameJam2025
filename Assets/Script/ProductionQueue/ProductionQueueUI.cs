using System.Collections.Generic;
using UnityEngine;

public class ProductionQueueUI : MonoBehaviour
{
    public static ProductionQueueUI Instance { get; private set; }


    public void Instantiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple ProductionQueueUI instances detected");
            Destroy(gameObject);
        }
    }
    
    [Header("UI Elements")]
    public GameObject currentProductionDisplay;
    public Transform queueLayoutGroup;
    
    [Header("Prefabs")]
    public GameObject productionItemPrefab;

    /// <summary>
    /// Updates the UI to display current production and production queue
    /// </summary>
    public void UpdateUI(CityProductionQueue cityProductionQueue)
    {
        if (cityProductionQueue == null)
        {
            Debug.LogError("CityProductionQueue reference is null");
            return;
        }

        if (currentProductionDisplay == null)
        {
            Debug.LogError("CurrentProductionDisplay reference is null");
            return;
        }

        if (queueLayoutGroup == null)
        {
            Debug.LogError("QueueLayoutGroup reference is null");
            return;
        }

        // Update current production display
        Production currentProduction = cityProductionQueue.currentProduction;
        
        if (currentProduction != null)
        {
            currentProductionDisplay.SetActive(true);
            ProductionQueueItem itemComponent = currentProductionDisplay.GetComponent<ProductionQueueItem>();
            if (itemComponent != null)
            {
                itemComponent.SetupItem(currentProduction);
            }
            else
            {
                Debug.LogError("CurrentProductionDisplay does not have ProductionQueueItem component");
            }
        }
        else
        {
            currentProductionDisplay.SetActive(false);
        }

        // Clear existing queue items
        ClearQueueDisplay();

        // Update production queue display
        List<Production> productionQueue = cityProductionQueue.productionQueue;
        
        if (productionQueue != null && productionQueue.Count > 0)
        {
            foreach (Production production in productionQueue)
            {
                if (production != null)
                {
                    GameObject queueItem = CreateQueueItem(production);
                    if (queueItem != null)
                    {
                        queueItem.transform.SetParent(queueLayoutGroup, false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a queue item for display
    /// </summary>
    private GameObject CreateQueueItem(Production production)
    {
        if (productionItemPrefab == null)
        {
            Debug.LogError("ProductionItemPrefab is not assigned");
            return null;
        }

        GameObject queueItem = Instantiate(productionItemPrefab);
        ProductionQueueItem itemComponent = queueItem.GetComponent<ProductionQueueItem>();
        
        if (itemComponent != null)
        {
            itemComponent.SetupItem(production);
        }
        else
        {
            Debug.LogError("ProductionItemPrefab does not have ProductionQueueItem component");
        }
        
        return queueItem;
    }

    /// <summary>
    /// Clears all queue display items
    /// </summary>
    private void ClearQueueDisplay()
    {
        if (queueLayoutGroup == null)
        {
            return;
        }

        foreach (Transform child in queueLayoutGroup)
        {
            Destroy(child.gameObject);
        }
    }
}

