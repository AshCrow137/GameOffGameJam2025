using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseTreasureChestEvent", menuName = "BaseTreasureChestEvent/TreasureResourceEvent")]
public class TreasureChestResourceEvent : BaseTreasureChestEvent
{
    public List<ResourceType> resources;
    public bool isIncremented;
    public int minResource;
    public int maxResource;

    public override void ExecuteEvent(BaseKingdom kingdom, Vector3Int chestPos)
    {
        base.ExecuteEvent(kingdom,chestPos);
        int amount = Random.Range(minResource, maxResource);
        ResourceType resource = resources[Random.Range(0, resources.Count)];

        if (!isIncremented)
        {
            amount *= -1;
        }

        kingdom.Resources().AddAll(new Dictionary<ResourceType, int> { { resource, amount } });

        Debug.Log($"Gain {amount} {resource}");

        if (kingdom is PlayerKingdom)
        {
            string text;
            if (isIncremented)
            {
                text = $"You Receive {amount} of {resource}.";
            }
            else
            {
                text = $"You Lost {Mathf.Abs(amount)} of {resource}.";
            }
            UIManager.Instance.ShowGamePlayEvent(text);
        }
    }


}