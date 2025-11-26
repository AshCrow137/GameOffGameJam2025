using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class GameplayCanvasManager : MonoBehaviour
{
    public static GameplayCanvasManager instance { get; private set; }
    [SerializeField]
    private GameObject productionPanel;
    //[SerializeField]
    //private GameObject buildingProductionPanel;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private float showMessageTime = 3;
    [SerializeField]
    private GameObject victoryPanel;
    [SerializeField]
    private GameObject wavecallerButton;
    private BaseGridUnitScript selectedUnit;
    public bool isOnCanvas = false;

    [HideInInspector]
    public GridCity selectedCity { get; private set; } = null;
    private IEnumerator showMessageCoroutine;
    public void Initialize()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        GlobalEventManager.ShowUIMessageEvent.AddListener(ShowMessageText);
        GlobalEventManager.KingdomDefeatEvent.AddListener(OnVictory);
    }
    public void OnVictory(BaseKingdom kingdom)
    {
        victoryPanel.SetActive(true);
        StartCoroutine(winScreenCoroutine());
    }
    private IEnumerator winScreenCoroutine()
    {
        yield return new WaitForSeconds(showMessageTime);
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void ActivateUnitProductionPanel(GridCity city)
    {
        productionPanel.SetActive(true);
        selectedCity = city;
    }
    public void DeactivateUnitProductionPanel()
    {
        productionPanel.SetActive(false);
        selectedCity = null;
    }
    public void ShowMessageText(string message)
    {
        messageText.text = message;
        if (showMessageCoroutine != null)
        {
            StopCoroutine(showMessageCoroutine);
            showMessageCoroutine = null;
        }
        showMessageCoroutine = ShowMessageCoroutine();
        StartCoroutine(showMessageCoroutine);
    }
    private IEnumerator ShowMessageCoroutine()
    {
        messageText.gameObject.SetActive(true);
        float t = showMessageTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        messageText.gameObject.SetActive(false);

    }
    public void TryToSpawnUnit(GameObject unitPrefab)
    {
        selectedCity?.TryToSpawnUnitInCity(unitPrefab);
    }
    public void TryToSpawnUnit(GridCity selectedOutsideCity,GameObject unitPrefab)
    {
        selectedOutsideCity?.TryToSpawnUnitInCity(unitPrefab);
    }
    public void OnMouseEnterCanvasElement()
    {
        InputManager.instance.SetOnUiElement(true);
        isOnCanvas = true;
    }
    public void OnMouseExitCanvasElement()
    {
        InputManager.instance.SetOnUiElement(false);
        isOnCanvas = false;
    }
    public void ActivateWavecallerButton(BaseGridUnitScript unit)
    {
        //wavecallerButton.SetActive(true);
        selectedUnit = unit;
    }
    public void DeactivateWavecallerButton()
    {
        //wavecallerButton.SetActive(false);
        selectedUnit = null;
    }
    public void CallSpecialAbility()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SpecialAbility();
        }
    }
    public void OnTileChosen(CallbackContext context)
    {
        if (!context.performed) return;
        if (selectedUnit == null) return;
        Debug.Log(selectedUnit.entityType);
        Debug.Log(selectedUnit.aiming);
        if (selectedUnit.entityType == EntityType.Special && selectedUnit.aiming == true && isOnCanvas == false)
        {
            selectedUnit.OnChosingTile();
        }
    }
}