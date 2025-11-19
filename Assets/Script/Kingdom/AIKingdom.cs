using UnityEngine;

// Ai controlled kingdom class
public class AIKingdom : BaseKingdom
{
    int relationsWithPlayer;

    [SerializeField]
    private MadnessData madnessData;

    private MadnessDataStruct currentMadnessEffect;

    protected override void OnStartTurn(BaseKingdom kingdom)
    {
        base.OnStartTurn(kingdom);
    }
    protected override void OnEndTurn(BaseKingdom kingdom)
    {
        base.OnEndTurn(kingdom);
    }

    public override void IncreaseMadness(int amount)
    {
        base.IncreaseMadness(amount);
        currentMadnessEffect = madnessData.GetMadnessEffects(madnessLevel);
        
        foreach(BaseGridUnitScript unit in controlledUnits)
        {
            unit.ApplyMadnessEffect(currentMadnessEffect);
        }
        
    }
    public override void DecreaseMadness(int amount)
    {
        base.DecreaseMadness(amount);
        currentMadnessEffect = madnessData.GetMadnessEffects(amount);

        foreach (BaseGridUnitScript unit in controlledUnits)
        {
            unit.ApplyMadnessEffect(currentMadnessEffect);
        }
    }
}