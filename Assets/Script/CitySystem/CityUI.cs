using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

[Serializable]
public class PlayerEntityPair
{
    public Button button;
    public GameObject entityPrefab;
}

/// <summary>
/// Manages city placement and operations on the grid
/// Usage: see testPlaceBuilding function for usage
/// </summary>
public class CityUI : MonoBehaviour
{
    [SerializeField]
    private GameObject cityUIPanel;
    [SerializeField]
    private PlayerKingdom playerKingdom;
    
    [SerializeField]
    private List<PlayerEntityPair> unitButtons = new List<PlayerEntityPair>();
    
    // [SerializeField]
    // private Button spawnUnitButton;
    // [SerializeField]
    // private Button spawnBuildingButton;
    //get; set;
    public CityMenuMode cityMenuMode { get; private set; } = CityMenuMode.None;

    public bool isUsingCityMenu { get; private set; } = false;

    //singleton
    public static CityUI Instance { get; private set; }
    private int buildingType;
    private GameObject unitPrefab;
    
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

    public void UpdateUnitButtonsInteractability()
    {
        foreach (var pair in unitButtons)
        {
            BaseGridEntity unitScript = pair.entityPrefab.GetComponent<BaseGridEntity>();

            List<BaseGridEntity> unlockedEntities = new();
            unlockedEntities.AddRange(playerKingdom.GetunlockedUnits());
            unlockedEntities.AddRange(playerKingdom.GetUnlockedBuildings());

            bool isUnlocked = false;

            foreach (var unlockedEntity in unlockedEntities)
            {
                if (unlockedEntity.GetType() == unitScript.GetType())
                {
                    isUnlocked = true;
                    break;
                }
            }

            pair.button.interactable = isUnlocked;
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
    private void ShowGreenTIles()
    {
        GridCity selectedCity = GameplayCanvasManager.instance.selectedCity;
        List<Vector3Int> positions = HexTilemapManager.Instance.GetCellsInRange(selectedCity.GetCellPosition(), selectedCity.unitSpawnRadius, new List<TileState> { TileState.Land, TileState.Water });
        foreach (Vector3Int pos in positions)
        {
            HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Green);
        }
    }
    public void SetSpawnUnitMode(GameObject unitPrefab)
    {
        List<BaseGridUnitScript> unlockedUnits =  GameplayCanvasManager.instance.selectedCity.GetOwner().GetunlockedUnits();
        BaseGridUnitScript prefabScript = unitPrefab.GetComponent<BaseGridUnitScript>();
        bool bUnitUnlocked = false;
        foreach(BaseGridUnitScript unit in unlockedUnits)
        {
            if(unit.GetType()==prefabScript.GetType())
            {
                bUnitUnlocked = true;
                break;
            }
        }
        if(bUnitUnlocked)
        {
            cityMenuMode = CityMenuMode.SpawnUnit;
            isUsingCityMenu = true;
            this.unitPrefab = unitPrefab;
            GameplayCanvasManager.instance.selectedCity.UpdateUnitSpawnRadius();
            AudioManager.Instance.ui_menumain_volume.Post(gameObject);
            ShowGreenTIles();
        }
        else
        {
            GlobalEventManager.InvokeShowUIMessageEvent("You did not unlock this unit!");
        }

    }

    public void SetSpawnBuildingMode(int BuildingType)
    {
        cityMenuMode = CityMenuMode.SpawnBuilding;
        isUsingCityMenu = true;
        buildingType = BuildingType;
        GameplayCanvasManager.instance.selectedCity.UpdateUnitSpawnRadius();
        AudioManager.Instance.ui_menumain_volume.Post(gameObject);
        ShowGreenTIles();
        //HexTilemapManager.Instance.PlaceColoredMarkerOnPosition
    }

    private void ClearCityMenuMode()
    {
        UIManager.Instance.HideEntityProductionPanelInfo();
        cityMenuMode = CityMenuMode.None;

        // StartCoroutine(ClearCityMenuModeDelayed());
    }

    // private IEnumerator ClearCityMenuModeDelayed()
    // {
    //     yield return new WaitForSeconds(5f);
    //     isUsingCityMenu = false;
    // }
    private City selectedCity;
    public void OnCitySelected(City city)
    {
        ShowCityUI(city);
        UpdateCityUI(city);
        selectedCity = city;
    }

    public void OnCityDeselected()
    {
        HideCityUI();
        ClearCityMenuMode();
        if (selectedCity != null)
        {
            HexTilemapManager.Instance.RemoveAllMarkers();
            selectedCity = null;
        }

    }

    public void OnClick(CallbackContext context)
    {
        if (!context.performed) return;
        if (InputManager.instance.IsCursorOverUIElement()) return;
        if (cityMenuMode == CityMenuMode.SpawnUnit)
        {
            UnitSpawner.Instance.QueueUnitAtMousePosition(GameplayCanvasManager.instance.selectedCity, unitPrefab);
            //TODO replace with actual sound
            AudioManager.Instance.ui_menumain_exit.Post(gameObject);
            HexTilemapManager.Instance.RemoveAllMarkers();
        }
        else if (cityMenuMode == CityMenuMode.SpawnBuilding)
        {
            BuildingManager.Instance.QueueBuildingAtMousePosition(GameplayCanvasManager.instance.selectedCity, buildingType);
            //TODO replace with actual sound
            AudioManager.Instance.ui_menumain_exit.Post(gameObject);
            HexTilemapManager.Instance.RemoveAllMarkers();
            // BuildingManager.Instance.PlaceBuildingAtMousePosition(GameplayCanvasManager.instance.selectedCity);
        }
        ClearCityMenuMode();

    }



}