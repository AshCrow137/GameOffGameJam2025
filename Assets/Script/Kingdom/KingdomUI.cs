using UnityEngine;
using System.Collections.Generic;

// Base kingdom class
public class BaseKingdom : MonoBehaviour
{
    public static KingdomUI Instance { get; private set; }
    public void Instantiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField]
    private PlayerKingdom playerKingdom;

    [SerializeField]
    private GameObject kingdomUIPanel;

    private void ShowKingdomUI(BaseKingdom kingdom){
        kingdomUIPanel.SetActive(true);
    }

    private void HideKingdomUI(){
        kingdomUIPanel.SetActive(false);
    }

    public void OnTurnStart(){
        if(TurnManager.instance.GetCurrentActingKingdom() == playerKingdom){
            ShowKingdomUI();
        }else{
            HideKingdomUI();
            StopPlaceCityMode();
        }
    }
    
    private bool shouldPlaceCity = false;
    public void StartPlaceCityMode()
    {
        shouldPlaceCity = true;
    }

    public void StopPlaceCityMode()
    {
        shouldPlaceCity = false;
    }

    public void OnClick(CallbackContext context)
    {
        if(!context.performed) return;
        if (shouldPlaceCity)
        {
            CityManager.Instance.PlaceCityAtMousePosition(playerKingdom);
        }
        shouldPlaceCity = false;
    }




}