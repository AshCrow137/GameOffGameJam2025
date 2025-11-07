using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Manages city placement and operations on the grid
/// Usage: see testPlaceBuilding function for usage
/// </summary>
public class CityUI : MonoBehaviour
{
    [SerializeField]
    private GameObject cityUIPanel;
    // [SerializeField]
    // private Button spawnUnitButton;
    // [SerializeField]
    // private Button spawnBuildingButton;
    //get; set;
    public CityMenuMode cityMenuMode { get; private set; } = CityMenuMode.None;

    public bool isUsingCityMenu { get; private set; } = false;

    //singleton
    public static CityUI Instance { get; private set; }
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

    
    public void ShowCityUI(City city)
    {
        cityUIPanel.SetActive(true);
    }

    private void HideCityUI()
    {
        cityUIPanel.SetActive(false);
    }
    private void UpdateCityUI(City city)
    {
        // Placeholder for UI logic
    }

    public void SetSpawnUnitMode()
    {
        cityMenuMode = CityMenuMode.SpawnUnit;
        isUsingCityMenu = true;
    }

    public void SetSpawnBuildingMode()
    {
        cityMenuMode = CityMenuMode.SpawnBuilding;
        isUsingCityMenu = true;
    }

    private void ClearCityMenuMode()
    {
        cityMenuMode = CityMenuMode.None;
        // StartCoroutine(ClearCityMenuModeDelayed());
    }

    // private IEnumerator ClearCityMenuModeDelayed()
    // {
    //     yield return new WaitForSeconds(5f);
    //     isUsingCityMenu = false;
    // }

    public void OnCitySelected(City city)
    {
        ShowCityUI(city);
        UpdateCityUI(city);
    }

    public void OnCityDeselected()
    {
        HideCityUI();
        ClearCityMenuMode();
    }

    public void OnClick(CallbackContext context)
    {
        if(!context.performed) return;
        if (cityMenuMode == CityMenuMode.SpawnUnit)
        {
            CityManager.Instance.SpawnUnitAtMousePosition(SelectionManager.Instance.selectedCity);
        }
        else if (cityMenuMode == CityMenuMode.SpawnBuilding)
        {
            BuildingManager.Instance.PlaceBuildingAtMousePosition(SelectionManager.Instance.selectedCity);
        }
        ClearCityMenuMode();
        
    }



}