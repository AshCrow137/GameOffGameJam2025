using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductionQueueItem : MonoBehaviour
{
    [Header("UI Components")]
    public Image productionIcon;
    public TextMeshProUGUI turnsLeftText;

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
    }
}


