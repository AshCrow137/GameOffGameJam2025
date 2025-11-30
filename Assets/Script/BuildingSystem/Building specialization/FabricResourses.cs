using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class FabricResourses : GridBuilding
{
    [Header("Production Data")]
    [SerializeField] private int productGold;
    [SerializeField] private int productMagic;
    [SerializeField] private int productMaterial;
    Dictionary<ResourceType, int> product = new Dictionary<ResourceType, int>();

    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        if (productGold > 0) product.Add(ResourceType.Gold, productGold);
        if (productMagic > 0) product.Add(ResourceType.Magic, productMagic);
        if (productMaterial > 0) product.Add(ResourceType.Materials, productMaterial);
    }
    protected override void OnStartTurn(BaseKingdom kingdom)
    {
        base.OnStartTurn(kingdom);
        if (kingdom != Owner) { return; }
        if (!bIsActive) return;
        Debug.Log("add resource");
        

        Owner.Resources().AddAll(product);
    }
    public Dictionary<ResourceType, int> GetProduction()
    {
        return new Dictionary<ResourceType, int>() { {ResourceType.Magic, productMagic }, { ResourceType.Gold, productGold }, { ResourceType.Materials, productMaterial} };
    }
    public override string GetBuildingFunction()
    {
        string text = "Produce";
        if (productGold > 0) text+= $" {productGold} gold";
        if (productMagic > 0) text += $" {productMagic} magic";
        if (productMaterial > 0) text += $" {productMaterial} materials";
        return text;
    }
}
