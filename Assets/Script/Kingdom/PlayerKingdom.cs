using UnityEngine;

// Player controlled kingdom class
public class PlayerKingdom : BaseKingdom
{

    public MadnessEffect GetMadnessEffects()
    {
        return MadnessEffect.None;
    }

    public void StartTurn()
    {
        KingdomUI.Instance?.StartTurn();
    }
   
}