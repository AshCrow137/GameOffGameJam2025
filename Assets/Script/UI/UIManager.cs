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
    private Image panelStats;

    [SerializeField]
    private Image infantaryType;

    [SerializeField]
    private Image infantaryImg;

    [SerializeField]
    private Image essenceImg;
    [SerializeField]
    private TextMeshProUGUI essenceValue;

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

        habiliyInfo.text = "nothing yet";

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
        MadnessLavel.text = city.GetOwner().madnessLevel.ToString();

    }

    public void UnitsInteractable(bool value)
    {
        foreach (Button button in UIElements.UnitsProduction)
        {
            button.interactable = value;
        }
    }

    public void ChangeTurn(int currentTurn, int totalTurns)
    {
        TurnCount.text = (currentTurn + 1).ToString() + " / " + totalTurns.ToString();
        UnitsInteractable(true);
    }

    public void UpdateLife(BaseGridUnitScript unit)
    {
        LifeBar.fillAmount = (float)unit.GetCurrentHealth() / (float)unit.GetMaxHealth();
    }

    public void ChangeEssence(int essence)
    {
        int index = 0;
        essenceValue.text = essence.ToString();
        switch (essence)
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
        essenceImg.sprite = UIElements.EssenceImgs[index];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        ChangeEssence((int)timer);
        if(timer >= 120)
        {
            timer = 0;
        }

    }
}
