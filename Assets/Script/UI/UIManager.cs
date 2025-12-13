using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private UIElements UIElements;
    float timer = 0;

    [SerializeField]
    private Canvas mainCanvas;

    [SerializeField]
    private Image CityPanel;
    [SerializeField]
    private TextMeshProUGUI MadnessLavel;
    [SerializeField]
    private Image madnessFillImage;

    [SerializeField]
    private Image panelStats;

    [SerializeField]
    private Image infantaryType;

    [SerializeField]
    private Image infantaryImg;

    [SerializeField]
    private Image magicResourceImg;
    [SerializeField]
    private Image goldResourceImg;
    [SerializeField]
    private Image materialsResourceImg;
    [SerializeField]
    private TextMeshProUGUI magicResourceText;
    [SerializeField]
    private TextMeshProUGUI goldResourceText;
    [SerializeField]
    private TextMeshProUGUI materialsResourceText;
    [SerializeField]
    private GameObject GamePlayEvents;
    [SerializeField]
    private Image NextTurnImg;
    [SerializeField]
    private TextMeshProUGUI TurnCount;

    [Header("Stats Texts")]
    [SerializeField]
    private TextMeshProUGUI unitName;
    [SerializeField]
    private TextMeshProUGUI unitHealthText;
    [SerializeField]
    private TextMeshProUGUI meleeAtack;
    [SerializeField]
    private TextMeshProUGUI rangedAtack;
    [SerializeField]
    private TextMeshProUGUI retaliationAtack;
    [SerializeField]
    private TextMeshProUGUI atackDistance;
    [SerializeField]
    private TextMeshProUGUI habiliyInfo;
    [SerializeField]
    private GameObject AbilitiesPanel;
    [SerializeField]
    private Image abilityImage;
    [SerializeField]
    private TMP_Text abilityText;

    [SerializeField]
    private Image LifeBar;

    [SerializeField]
    private GameObject UnitProductionButton;
    [SerializeField]
    private GameObject BuildingProductionButton;
    [SerializeField]
    private GameObject UnitProductionPanel;
    [SerializeField]
    private GameObject BuildingProductionPanel;

    [SerializeField]
    private GameObject ProductionPanel;
    [SerializeField]
    private GameObject ProductionFirstItemPanel;


    [SerializeField]
    private GameObject HintObject;

    [Header("ProductionInfo")]
    [SerializeField]
    private GameObject CityProductionInfoPanel;
    [SerializeField]
    private TMP_Text PI_EntityDescription;
    [SerializeField]
    private TMP_Text PI_EntityName;
    [SerializeField]
    private TMP_Text PI_EntityMagicCost;
    [SerializeField]
    private TMP_Text PI_EntityGoldCost;
    [SerializeField]
    private TMP_Text PI_EntityMaterialsCost;
    [SerializeField]
    private TMP_Text PI_BuildingDuration;
    [SerializeField]
    private RectTransform PI_RequiredBuildingsPanel;
    [SerializeField]
    private GameObject PI_RequiredBuildingImagePrefab;
    [SerializeField]
    private GameObject PI_UnitStatsPanel;
    [SerializeField]
    private GameObject PI_BuildingStatsPanel;
    [SerializeField]
    private TMP_Text PI_UnitHealth;
    [SerializeField]
    private TMP_Text PI_UnitMeleeAttack;
    [SerializeField]
    private TMP_Text PI_UnitRangeAttack;
    [SerializeField]
    private TMP_Text PI_UnitRetallitionAttack;
    [SerializeField]
    private TMP_Text PI_UnitAttackRange;
    [SerializeField]
    private TMP_Text PI_UnitMovementDistance;
    [SerializeField]
    private TMP_Text PI_BuildingAddHPToCity;
    [SerializeField]
    private TMP_Text PI_BuildingFunction;
    [SerializeField]
    private GameObject productionPanel;

    private UIElementHints currentHints;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private float showMessageTime = 3;
    [SerializeField]
    private GameObject victoryPanel;
    [SerializeField]
    private GameObject losePanel;
    private IEnumerator showMessageCoroutine;
    public bool isOnCanvas { get; private set; } = false;
    public void Initialize()
    {
        UIElements = GetComponent<UIElements>();

        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        panelStats.gameObject.SetActive(false);
        NextTurnImg.sprite = UIElements.EnemyTurn;
        //GlobalEventManager.ShowUIMessageEvent.AddListener(ShowMessageText);
    }

    public void HasUnitSelected(bool value)
    {
        panelStats.gameObject.SetActive(value);
    }

    public void OnUnitSelect(BaseGridUnitScript unit)
    {
        CityProductionInfoPanel.SetActive(false);
        if (unit == null) return;
        HasUnitSelected(true);

        if (unit.GetOwner() == null) return;

        PlayerKingdom pk = unit.GetOwner().gameObject.GetComponent<PlayerKingdom>();

        if (pk != null)
        {
            panelStats.sprite = UIElements.PlayerPannel;
            infantaryType.sprite = UIElements.PlayerType;
        }
        else
        {
            panelStats.sprite = UIElements.EnemyPannel;
            infantaryType.sprite = UIElements.EnemyType;
        }
        infantaryImg.sprite = unit.gameObject.transform.Find("BodySprite").GetComponent<SpriteRenderer>().sprite;

        unitName.text = unit.GetEntityDisplayName();
        unitHealthText.text = $"{unit.GetCurrentHealth()}/{unit.GetMaxHealth()}";
        meleeAtack.text = unit.GetMeleeDamage().ToString();
        rangedAtack.text = unit.GetRangeAttackDamage().ToString();
        retaliationAtack.text = unit.GetRetaliationDamage().ToString();
        atackDistance.text = unit.GetAtackDistance().ToString();
        Sprite unitAbilityImage = unit.GetAbilityImage();
        string unitAbilityDescription = unit.GetAbilityDescription();
        if (unitAbilityImage != null && unitAbilityDescription != null)
        {
            AbilitiesPanel.SetActive(true);
            abilityImage.sprite = unitAbilityImage;
            abilityText.text = unitAbilityDescription;
        }
        else
        {
            AbilitiesPanel.SetActive(false);
        }
        //habiliyInfo.text = "nothing yet";

        UpdateLife(unit);
    }
    public void ActivateUnitAbility(int abilityIndex)
    {
        UIUtility.selectedUnit?.SpecialAbility();
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
    public void HasCitySelected(bool value)
    {
        CityPanel.gameObject.SetActive(value);
    }

    public void OnCitySelect(GridCity city)
    {
        CityProductionInfoPanel.SetActive(false);
        if (city == null) return;
        HasCitySelected(true);

        CityPanel.sprite = UIElements.CityPanel;
        MadnessLavel.text = $"Madness: {city.GetOwner().madnessLevel}";
        madnessFillImage.fillAmount = (float)city.GetOwner().madnessLevel / city.GetOwner().maxMadnessLevel;
        if (city.GetOwner() is AIKingdom)
        {
            AIKingdom ai = (AIKingdom)city.GetOwner();
            if (ai.GetCurrentMadnessEffect().VisibleProduction)
            {
                ProductionFirstItemPanel?.SetActive(true);
                ProductionPanel?.SetActive(true);
            }
            else
            {
                ProductionFirstItemPanel?.SetActive(false);
                ProductionPanel?.SetActive(false);
            }
            UnitProductionButton?.SetActive(false);
            BuildingProductionButton?.SetActive(false);
            UnitProductionPanel?.SetActive(false);
            BuildingProductionPanel?.SetActive(false);
        }
        else
        {
            UnitProductionButton?.SetActive(true);
            BuildingProductionButton?.SetActive(true);
            ProductionFirstItemPanel?.SetActive(true);
            ProductionPanel?.SetActive(true);
        }
        //CityUI.Instance.UpdateUnitButtonsInteractability();
    }

    public void UnitsInteractable(bool value)
    {
        foreach (Button button in UIElements.UnitsProduction)
        {
            button.interactable = value;
        }
    }

    public void ChangeTurn(int currentTurn)
    {
        TurnCount.text = (currentTurn).ToString();
        ChangeNextTurnImg();
        UnitsInteractable(true);
    }

    private void ChangeNextTurnImg()
    {
        NextTurnImg.sprite = TurnManager.instance.GetCurrentActingKingdom() == PlayerKingdom.Instance ? UIElements.EnemyTurn : UIElements.PlayerTurn;
    }

    public void UpdateLife(BaseGridUnitScript unit)
    {
        LifeBar.fillAmount = (float)unit.GetCurrentHealth() / (float)unit.GetMaxHealth();
    }

    public void UpdateResourceImages(int resourceValue, ResourceType resourceType)
    {
        Image resource = null;
        TextMeshProUGUI text = null;
        switch (resourceType)
        {
            case ResourceType.Magic:
                resource = magicResourceImg;
                text = magicResourceText;
                break;
            case ResourceType.Gold:
                resource = goldResourceImg;
                text = goldResourceText;
                break;
            case ResourceType.Materials:
                resource = materialsResourceImg;
                text = materialsResourceText;
                break;
        }
        int index = 0;
        text.text = resourceValue.ToString();
        switch (resourceValue)
        {
            case < 50:
                index = 0;
                break;
            case < 100:
                index = 1;
                break;
            case < 150:
                index = 2;
                break;
            case < 200:
                index = 3;
                break;
            case < 250:
                index = 4;
                break;
            case < 300:
                index = 5;
                break;
            case < 350:
                index = 6;
                break;
            case >= 400:
                index = 7;
                break;
        }
        resource.sprite = UIElements.EssenceImgs[index];
    }
    public void SwapUIElementState(GameObject UIElement)
    {
        UIElement.SetActive(!UIElement.activeSelf);
        AudioManager.Instance.ui_menumain_volume.Post(gameObject);
    }

    public void ShowEntityDescription(BaseGridEntity entity)
    {

    }

    public void ShowGamePlayEvent(string text)
    {
        GamePlayEvents.SetActive(true);
        TextMeshProUGUI panelText = GamePlayEvents.GetComponentInChildren<TextMeshProUGUI>();
        panelText.text = text;

        StartCoroutine("ShowOffGamePlayEvents");
    }

    IEnumerator ShowOffGamePlayEvents()
    {
        yield return new WaitForSeconds(3);

        GamePlayEvents.SetActive(false);
    }

    public GameObject GetHintObject()
    {
        return HintObject;
    }

    public void ShowCityProductionEntityInfo(BaseGridEntity entity)
    {
        CityProductionInfoPanel.SetActive(true);
        PI_EntityName.text = entity.GetEntityDisplayName();
        PI_EntityDescription.text = entity.GetEntityDescription();

        if (entity is BaseGridUnitScript)
        {
            BaseGridUnitScript unit = (BaseGridUnitScript)entity;
            PI_EntityMagicCost.text = unit.resource.TryGetValue(ResourceType.Magic, out int mvalue) ? mvalue.ToString() : "0";
            PI_EntityGoldCost.text = unit.resource.TryGetValue(ResourceType.Gold, out int gvalue) ? gvalue.ToString() : "0";
            PI_EntityMaterialsCost.text = unit.resource.TryGetValue(ResourceType.Materials, out int matvalue) ? matvalue.ToString() : "0";
            PI_BuildingDuration.text = unit.GetProductionTime().ToString();
            PI_UnitStatsPanel.SetActive(true);
            PI_BuildingStatsPanel.SetActive(false);
            PI_UnitHealth.text = unit.GetRawMaxHealth().ToString();
            PI_UnitMeleeAttack.text = unit.GetRawMeleeDamage().ToString();
            PI_UnitRangeAttack.text = unit.GetRawRangedAttackDamage().ToString();
            PI_UnitRetallitionAttack.text = unit.GetRawRetallitionDamage().ToString();
            PI_UnitAttackRange.text = unit.GetAtackDistance().ToString();
            PI_UnitMovementDistance.text = unit.GetMovementDistance().ToString();

            List<GridBuilding> requredBuildings = unit.GetRequiredBuildings();
            if (requredBuildings.Count > 0)
            {

                Transform child = PI_RequiredBuildingsPanel.transform.GetChild(0);
                child.gameObject.SetActive(true);
                Image image = child.gameObject.GetComponent<Image>();
                image.sprite = requredBuildings[0].GetBuildingUISprite();
            }
            else
            {
                PI_RequiredBuildingsPanel.transform.GetChild(0).gameObject.SetActive(false);
                PI_RequiredBuildingImagePrefab.SetActive(false);
            }
        }
        else if (entity is GridBuilding)
        {
            PI_UnitStatsPanel.SetActive(false);
            PI_BuildingStatsPanel.SetActive(true);
            GridBuilding building = (GridBuilding)entity;
            PI_EntityMagicCost.text = building.GetBuilding().resource.TryGetValue(ResourceType.Magic, out int mvalue) ? mvalue.ToString() : "0";
            PI_EntityGoldCost.text = building.GetBuilding().resource.TryGetValue(ResourceType.Gold, out int gvalue) ? gvalue.ToString() : "0";
            PI_EntityMaterialsCost.text = building.GetBuilding().resource.TryGetValue(ResourceType.Materials, out int matvalue) ? matvalue.ToString() : "0";
            PI_BuildingFunction.text = building.GetBuildingFunction();

        }
    }
    public void HideEntityProductionPanelInfo()
    {
        CityProductionInfoPanel.SetActive(false);
    }
    public void DisableMainCanvas()
    {
        CanvasGroup group = mainCanvas.GetComponent<CanvasGroup>();
        group.interactable = false;
    }
    public void EnableMainCanvas()
    {
        CanvasGroup group = mainCanvas.GetComponent<CanvasGroup>();
        group.interactable = true;
    }
    public void ActivateUnitProductionPanel(GridCity city)
    {
        productionPanel.SetActive(true);

    }
    public void DeactivateUnitProductionPanel()
    {
        productionPanel.SetActive(false);

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
    public void ShowWinScreen()
    {
        victoryPanel.SetActive(true);
        StartCoroutine(winScreenCoroutine());
    }
    public void ShowLoseScreen()
    {
        losePanel.SetActive(true);
        StartCoroutine(winScreenCoroutine());
    }
    private IEnumerator winScreenCoroutine()
    {
        float t = showMessageTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadSceneAsync("ActualMainMenu");
    }
}
