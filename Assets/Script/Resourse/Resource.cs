using UnityEngine;
using System.Collections.Generic;
public enum ResourceType { Magic, Gold, Materials }
public class Resource 
{
    //public static Resource Instance { get; private set; }
    private Dictionary<ResourceType, int> resources = new();

    //private int StartingMagic = 50;
    //private int StartingGold = 20;
    //private int StartingMaterials = 30;

    public BaseKingdom Owner { get; private set; }
    public Resource(BaseKingdom owner,int StartingMagic,int StartingGold, int StartingMaterials)
    {
        resources[ResourceType.Magic] = StartingMagic;
        resources[ResourceType.Gold] = StartingGold;
        resources[ResourceType.Materials] = StartingMaterials;
        Owner = owner;
        if(Owner is PlayerKingdom)
        {
            UpdateUI();
        }
        
    }

    public void AddAll(Dictionary<ResourceType,int> required)
    {
        foreach (var req in required)
        {
            resources[req.Key] += req.Value;
            if (resources[req.Key] <0) 
            {
                resources[req.Key] = 0;
            }
            Debug.Log($"Add {req.Key} - {req.Value}");
        }
        UpdateUI();
    }

    public void SpendResource(Dictionary<ResourceType, int> required)
    {
        foreach (var req in required)
        {
            resources[req.Key] = Mathf.Max(0, resources[req.Key] - req.Value);
        }
        UpdateUI();
    }

    public void Remove( Dictionary<ResourceType,int> required)
    {
        
        foreach (var req in required)
        {
            resources[req.Key] = Mathf.Max(0, resources[req.Key] - req.Value);
        }
        UpdateUI();
    }

    public int Get(ResourceType type) => resources[type];

    public Dictionary<ResourceType,int> HasEnough(Dictionary<ResourceType, int> required)
    {
        var _temp = new Dictionary<ResourceType, int>();
        foreach (var req in required)
        {
            if (!resources.ContainsKey(req.Key) || resources[req.Key] < req.Value)
            {
                _temp.Add(req.Key, req.Value - resources[req.Key]);
            }

        }
        if (_temp.Count==0)
            return null;
        else
            return _temp;

    }
    private void UpdateUI()
    {
        if (Owner is PlayerKingdom)
        {
            foreach (KeyValuePair<ResourceType, int> kvp in resources)
            {
                UIManager.Instance.UpdateResourceImages(kvp.Value, kvp.Key);
            }
        }
            
    }

    public void TESTADDFORBUTTON()
    {
        Debug.Log($"Resources before adding\nMagic: {Get(ResourceType.Magic)}, Gold: {Get(ResourceType.Gold)}, Material: {Get(ResourceType.Materials)}");

        AddAll(new Dictionary<ResourceType, int>() {
            {ResourceType.Gold,10},
            {ResourceType.Magic,10},
            {ResourceType.Materials,10},
            });
        Debug.Log($"Resources after adding\nMagic: {Get(ResourceType.Magic)}, Gold: {Get(ResourceType.Gold)}, Material: {Get(ResourceType.Materials)}");
    }
    public void TESTREMOVEFORBUTTON()
    {
        Debug.Log($"Resources before removing\nMagic: {Get(ResourceType.Magic)}, Gold: {Get(ResourceType.Gold)}, Material: {Get(ResourceType.Materials)}");
        Dictionary<ResourceType, int> req = new Dictionary<ResourceType, int>
        {
            {ResourceType.Magic, 10},
            {ResourceType.Gold, 10},
        };
        var _temp = HasEnough(req);
        if (_temp == null)
        {
            Remove(req);
            Debug.Log($"Resources after removing\nMagic: {Get(ResourceType.Magic)}, Gold: {Get(ResourceType.Gold)}, Material: {Get(ResourceType.Materials)}");
        }
        else
        {
            foreach(var a in _temp)
            {
                Debug.Log($"not enough {a.Key} - {a.Value}");
            }
        }
        // if (HasEnough(req))
        // {
        //     Remove(new int[2] { 10, 10 });
        //     Debug.Log($"Resources after removing\nMagic: {Get(ResourceType.Magic)}, Gold: {Get(ResourceType.Gold)}, Material: {Get(ResourceType.Materials)}");
        // }

    }
}
