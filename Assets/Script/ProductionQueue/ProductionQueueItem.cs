using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ProductionQueueItem : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Components")]
    public Image productionIcon;
    public TextMeshProUGUI turnsLeftText;
    private Production production;


    /// <summary>
    /// Sets up the production queue item with production data
    /// </summary>
    public void SetupItem(Production production)
    {
        if (production == null)
        {
            Debug.LogError("Cannot setup ProductionQueueItem with null Production");
            return;
        }

        // Set the production icon if production type is building
        if (production.productionType == ProductionType.Building)
        {
            if (production.building != null)
            {
                if (productionIcon != null)
                {
                    productionIcon.sprite = production.building.sprite;
                }
                else
                {
                    Debug.LogError("ProductionIcon Image is not assigned");
                }
            }
            else
            {
                Debug.LogError("Production has Building type but building reference is null");
            }
        }

        // Set the turns left text
        if (turnsLeftText != null)
        {
            turnsLeftText.text = production.turnsRemaining.ToString();
        }
        else
        {
            Debug.LogError("TurnsLeftText is not assigned");
        }
        this.production = production;
    }

    /// <summary>
    /// Handles pointer click events - right-click to remove production
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameplayCanvasManager.instance.selectedCity.GetComponent<CityProductionQueue>().RemoveProduction(production);
        }
    }
}


