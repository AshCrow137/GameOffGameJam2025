using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Building", menuName = "Building System/Building Data")]
public class Building : ScriptableObject
{
    [Header("Building Information")]
    public string buildingName;
    public Sprite sprite;
    
    [Header("Resource Requirements")]
    [SerializeField] private List<ResourceRequirement> resourceRequirements = new List<ResourceRequirement>();
    
    [Header("Construction")]
    public int duration;
    // Property to access resources as Dictionary
    public Dictionary<ResourceType, int> resource
    {
        get
        {
            Dictionary<ResourceType, int> dict = new Dictionary<ResourceType, int>();
            foreach (var req in resourceRequirements)
            {
                dict[req.resourceType] = req.amount;
            }
            return dict;
        }
    }
}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceType resourceType;
    public int amount;
}

