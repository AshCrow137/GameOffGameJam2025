using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Image panelStats;

    private float timer = 0;
    private UIElements UIElements;

    public void Initialize()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        
        UIElements = GetComponent<UIElements>();
    }

    public void SelectedUnit(BaseGridUnitScript unit)
    {
        Debug.Log("Chegou aki");
        if (unit == null) return;

        if(unit.GetOwner() == null) return;

        PlayerKingdom pk = (PlayerKingdom)unit.GetOwner();

        if ((PlayerKingdom)unit.GetOwner())
        {
            panelStats.sprite = UIElements.EnemyPannel;
        }
        else
        {
            panelStats.sprite = UIElements.EnemyPannel;
        }
    }
}
