using UnityEngine;

public class TreasureChest : BaseGridEntity
{
    [Header("Treasure Chest Settings")]
    [SerializeField]
    private BaseGameplayEvent treasureEvent;
    
    private bool hasBeenCollected = false;

    /// <summary>
    /// Initialize the treasure chest - treasure chests are neutral entities
    /// </summary>
    public override void Initialize(BaseKingdom owner)
    {
        // Treasure chests cannot be attacked
        bCanBeAttacked = false;
        
        // Call base initialization (owner can be null for neutral chests)
        base.Initialize(owner);
        
        // Register this chest with the tilemap manager
        hTM.AddTreasureChestToTile(gridPosition, this);
    }

    /// <summary>
    /// Called when a unit collects this treasure chest
    /// </summary>
    /// <param name="collector">The kingdom that collected the chest</param>
    public void Collect(BaseKingdom collector)
    {
        if (hasBeenCollected || treasureEvent == null)
            return;

        hasBeenCollected = true;

        // Execute the treasure event for the collecting kingdom
        treasureEvent.ExecuteEvent(collector);
        
        // Show additional UI message for player
        //if (collector is PlayerKingdom)
        //{
        //    UIManager.Instance.ShowGamePlayEvent("You discovered a Treasure Chest!");
        //}

        // Remove from the grid and destroy
        Death();
        Destroy(gameObject);
    }

    public override void Death()
    {
        // Remove treasure chest from tilemap manager
        if (hTM != null)
        {
            hTM.RemoveTreasureChestFromTile(gridPosition);
        }
        
        // Call base death to remove from entity directory
        base.Death();
    }

    public BaseGameplayEvent GetTreasureEvent()
    {
        return treasureEvent;
    }
}



