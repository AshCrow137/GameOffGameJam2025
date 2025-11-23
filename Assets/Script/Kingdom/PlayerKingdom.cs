using UnityEngine;

// Player controlled kingdom class
public class PlayerKingdom : BaseKingdom
{
    public static PlayerKingdom Instance { get; private set; }

    //public void StartTurn()
    //{
    //    KingdomUI.Instance?.StartTurn();
    //}
    public override void IncreaseMadness(int amount)
    {
        
    }
    public override void Initialize()
    {
        base.Initialize();
        if(Instance!= null)
        {
            Debug.LogError("More than one player on scene!");
            Destroy(this);
            return;
        }
        Instance = this;
    }
}