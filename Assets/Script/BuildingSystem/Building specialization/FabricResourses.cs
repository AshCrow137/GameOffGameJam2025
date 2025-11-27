using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class FabricResourses : GridBuilding
{
    [Header("Production Data")]
    [SerializeField] private int productGold;
    [SerializeField] private int productMagic;
    [SerializeField] private int productMaterial;

    protected override void OnStartTurn(BaseKingdom kingdom)
    {
        base.OnStartTurn(kingdom);
        if (kingdom != Owner) { return; }
        Debug.Log("add resource");
        Dictionary<ResourceType, int> product = new Dictionary<ResourceType, int>();
        if (productGold > 0)    product.Add(ResourceType.Gold, productGold);
        if (productMagic > 0)   product.Add(ResourceType.Magic, productMagic);
        if (productMaterial > 0)product.Add(ResourceType.Materials, productMaterial);
        Owner.Resources().AddAll(product);
    }
    public Dictionary<ResourceType, int> GetProduction()
    {
        return new Dictionary<ResourceType, int>() { {ResourceType.Magic, productMagic }, { ResourceType.Gold, productGold }, { ResourceType.Materials, productMaterial} };
    }
}