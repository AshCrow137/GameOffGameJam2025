using UnityEngine;

[CreateAssetMenu(fileName = "BaseTreasureChestEvent", menuName = "BaseTreasureChestEvent")]
public class BaseTreasureChestEvent : BaseGameplayEvent
{

    public virtual void ExecuteEvent(BaseKingdom kingdom, Vector3Int chestPos)
    {
        base.ExecuteEvent(kingdom);
    }


}