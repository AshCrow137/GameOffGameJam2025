using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private UIElements UIElements;
    float timer = 0;

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
    private TextMeshProUGUI TurnCount;

    [Header("Status Texts")]
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


    public void Initialize()
    {
        UIElements = GetComponent<UIElements>();

        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        
        panelStats.gameObject.SetActive(false);
    }

    public void HasUnitSelected(bool value)
    {
        panelStats.gameObject.SetActive(value);
    }

    public void SelectedUnit(BaseGridUnitScript unit)
    {
        if (unit == null) return;
        HasUnitSelected(true);

        if(unit.GetOwner() == null) return;

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

        meleeAtack.text = unit.GetMeleeDamage().ToString();
        rangedAtack.text = unit.GetRangeAttackDamage().ToString();
        retaliationAtack.text = unit.GetRetaliationDamage().ToString();
        atackDistance.text = unit.GetAtackDistance().ToString();
        Sprite unitAbilityImage = unit.GetAbilityImage();
        string unitAbilityDescription = unit.GetAbilityDescription();
        if(unitAbilityImage != null&& unitAbilityDescription != null) 
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

    public void HasCitySelected(bool value)
    {
        CityPanel.gameObject.SetActive(value);
    }

    public void SelectedCity(GridCity city)
    {
        if(city == null) return;
        HasCitySelected(true);

        CityPanel.sprite = UIElements.CityPanel;
        MadnessLavel.text = $"Madness: {city.GetOwner().madnessLevel}";
        madnessFillImage.fillAmount = (float)city.GetOwner().madnessLevel / city.GetOwner().maxMadnessLevel;
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
        TurnCount.text = (currentTurn).ToString() ;
        UnitsInteractable(true);
    }

    public void UpdateLife(BaseGridUnitScript unit)
    {
        LifeBar.fillAmount = (float)unit.GetCurrentHealth() / (float)unit.GetMaxHealth();
    }

    public void UpdateResourceImages(int resourceValue,ResourceType resourceType)
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
            case < 12:
                index = 0;
                break;
            case < 24:
                index = 1;
                break;
            case < 35:
                index = 2; 
                break;
            case < 50:
                index = 3;
                break;
            case < 65:
                index = 4;
                break;
            case < 75:
                index = 5;
                break;
            case < 85:
                index = 6;
                break;
            case >= 85:
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
}
