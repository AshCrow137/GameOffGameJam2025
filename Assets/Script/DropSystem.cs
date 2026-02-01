using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    [SerializeField]
    private ResourceType[] resourcesToDrop;

    [SerializeField]
    private int minResource;

    [SerializeField]
    private int maxResource;

    public static DropSystem instance;

    public void Instantiate()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DropTo(BaseKingdom kingdom)
    {
        int amountOfResource = Random.Range(minResource, maxResource + 1);

        ResourceType resource = resourcesToDrop[Random.Range(0, resourcesToDrop.Length)];
        Debug.Log($"Drop {amountOfResource} of {resource} to {kingdom.name}");
        kingdom.Resources().AddAll(new Dictionary<ResourceType, int> { { resource, amountOfResource } });
    }
}
