using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        // Set the production icon if production type is building
        if (production.productionType == ProductionType.Building)
        {
            productionIcon.sprite = production.building.sprite;
        }
        else if (production.productionType == ProductionType.Unit)
        {
            productionIcon.sprite = production.prefab.GetComponent<BaseGridUnitScript>().GetSprite();
        }

        // Set the turns left text
        turnsLeftText.text = production.turnsRemaining.ToString();
        this.production = production;
    }

    /// <summary>
    /// Handles pointer click events - right-click to remove production
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CityProductionQueue queue = UIUtility.selectedCity.GetComponent<CityProductionQueue>();
            Production deleteProduction = production;
            queue.RemoveProduction(deleteProduction);
            deleteProduction.Cancel(queue.GetComponent<GridCity>());

        }
    }
}


