using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

// Base kingdom class
public class KingdomUI : MonoBehaviour
{
    public static KingdomUI Instance { get; private set; }

    public void Initialize()
    {
        Instantiate();
        InitializeEndTurn();
    }

    private void Instantiate()
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

    private void InitializeEndTurn(){
        GlobalEventManager.EndTurnEvent.AddListener(EndTurn);
    }
    [SerializeField]
    private PlayerKingdom playerKingdom;

    [SerializeField]
    private GameObject kingdomUIPanel;

    private void ShowKingdomUI(){
        kingdomUIPanel.SetActive(true);
    }

    private void HideKingdomUI(){
        kingdomUIPanel.SetActive(false);
    }

    public void StartTurn(){
        ShowKingdomUI();

        // if(TurnManager.instance.GetCurrentActingKingdom() == playerKingdom){
        //     ShowKingdomUI();
        // }
        // else{
        //     HideKingdomUI();
        //     StopPlaceCityMode();
        // }
    }
    
    public void EndTurn(BaseKingdom entity)
    {
        if(entity != playerKingdom)
            return;
        HideKingdomUI();
        StopPlaceCityMode();
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