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

<<<<<<< HEAD
        PlayerKingdom pk = (PlayerKingdom)unit.GetOwner();

        if (pk != null)
=======
        if ((PlayerKingdom)unit.GetOwner())
>>>>>>> parent of d6302b2 (.)
        {
            panelStats.sprite = UIElements.EnemyPannel;
        }
        else
        {
            panelStats.sprite = UIElements.EnemyPannel;
        }
    }
}
