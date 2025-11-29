using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSpecialEvent", menuName = "BaseGameplayEvent/SpawnSpecialEvent")]
public class SpawnSpecialEvents : BaseGameplayEvent
{
    public GameObject prefab;

    public override void ExecuteEvent(BaseKingdom kingdom)
    {

    }
}