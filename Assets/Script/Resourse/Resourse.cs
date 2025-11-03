using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
public enum ResourceType { Resource1, Resource2 }
public class Resourse : MonoBehaviour
{
    private Dictionary<ResourceType, int> resources = new();

    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        resources[ResourceType.Resource1] = 10;
        resources[ResourceType.Resource2] = 10;
    }

    public void AddAll(int[] value)
    {
        var resoursedKey = new List<ResourceType>(resources.Keys);
        for(int i = 0; i<resources.Count; i++)
        {
            resources[resoursedKey[i]] += value[i];
        }
    }
    public void Remove(int[] value)
    {
        var resoursedKey = new List<ResourceType>(resources.Keys);
        for(int i = 0; i<resources.Count; i++)
        {
            resources[resoursedKey[i]] = Mathf.Max(0, resources[resoursedKey[i]] - value[i]);
        }
    }

    public int Get(ResourceType type) => resources[type];

    public bool HasEnough(Dictionary<ResourceType, int> required)
    {
        foreach (var req in required)
        {
            if (!resources.ContainsKey(req.Key) || resources[req.Key] < req.Value)
                return false;
        }
        return true;
    }

    public void TESTADDFORBUTTON()
    {
        Debug.Log(Get(ResourceType.Resource1));
        Debug.Log(Get(ResourceType.Resource2));
        AddAll(new int[2] { 10, 20 });
        Debug.Log(Get(ResourceType.Resource1));
        Debug.Log(Get(ResourceType.Resource2));
    }
    public void TESTREMOVEFORBUTTON()
    {
        Debug.Log(Get(ResourceType.Resource1));
        Debug.Log(Get(ResourceType.Resource2));
        Dictionary<ResourceType, int> req = new Dictionary<ResourceType, int>
        {
            {ResourceType.Resource1, 10},
            {ResourceType.Resource2, 10}
        };

        if (HasEnough(req))
        {
            Remove(new int[2] { 10, 10 });
        }
    }
}
