using System.Collections.Generic;
using UnityEngine;

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
    public GameObject owner;
    public GameObject buildingPrefab;
    public GridCity ownerCity { get; private set; }
    [SerializeField] public AK.Wwise.Event buildingPlacementEvent;
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

    /// <summary>
    /// Creates a clone of this building with all its data
    /// </summary>
    public Building Clone()
    {
        Building clone = ScriptableObject.CreateInstance<Building>();
        clone.buildingName = this.buildingName;
        clone.sprite = this.sprite;
        clone.duration = this.duration;
        clone.owner = this.owner;

        // Clone resource requirements
        clone.resourceRequirements = new List<ResourceRequirement>();
        foreach (var req in this.resourceRequirements)
        {
            clone.resourceRequirements.Add(new ResourceRequirement
            {
                resourceType = req.resourceType,
                amount = req.amount
            });
        }

        return clone;
    }
    public void SetOwnerCity(GridCity newOwner)
    {
        ownerCity = newOwner;
    }
}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceType resourceType;
    public int amount;
}

