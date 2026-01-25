using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //turnManager = new TurnManager(players);
        //turnManager.Initialize();
        GlobalEventManager.KingdomDefeatEvent.AddListener(OnVictory);
    }
    public void OnVictory(BaseKingdom kingdom)
    {
        if (kingdom is PlayerKingdom)
        {
            UIManager.Instance.ShowLoseScreen();
        }
        else
        {
            TurnManager.instance.RemoveKingdomFromTurnOrder(kingdom);
            int kingdomsRemain = 0;
            foreach (BaseKingdom actingKingdom in TurnManager.instance.GetActingKingdoms())
            {
                if (actingKingdom is PlayerKingdom || actingKingdom is NeitralKingdom) continue;
                kingdomsRemain++;
            }
            if (kingdomsRemain == 0)
            {
                UIManager.Instance.ShowWinScreen();
            }

        }

    }

}
