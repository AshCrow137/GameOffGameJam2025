using UnityEngine;

[CreateAssetMenu(fileName = "SpawnAtPosition", menuName = "BaseTreasureChestEvent/SpawnAtPosition")]
public class SpawnUnitAtSpecificPosition : BaseTreasureChestEvent
{
    public GameObject meleePrefab;

    public override void ExecuteEvent(BaseKingdom kingdom, Vector3Int chestPos)
    {


        //instanciate the unit
        UnitSpawner.Instance.PlaceUnit(meleePrefab, chestPos, kingdom);

        if (kingdom is PlayerKingdom)
        {
            string text = "You Receive a unit.";
            UIManager.Instance.ShowGamePlayEvent(text);
        }

    }
}