using UnityEngine;

[CreateAssetMenu(fileName = "MadnessEvent", menuName = "BaseGameplayEvent/MadnessEvent")]
public class MadnessEvent : BaseGameplayEvent
{
    [SerializeField]
    private int minMadnessReduction;
    
    [SerializeField]
    private int maxMadnessReduction;

    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        int reductionAmount = Random.Range(minMadnessReduction, maxMadnessReduction);
        
        int previousMadness = kingdom.GetMadnessLevel();
        kingdom.DecreaseMadness(reductionAmount);
        int actualReduction = previousMadness - kingdom.GetMadnessLevel();

        Debug.Log($"Madness reduced by {actualReduction} (attempted {reductionAmount})");

        if (kingdom is PlayerKingdom)
        {
            string text = $"Your Madness decreased by {actualReduction}.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }
    }
}

