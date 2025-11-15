using UnityEngine;
using System.Collections.Generic;
public class FabricResourses : GridBuilding
{
    [Header("Production Data")]
    [SerializeField] private int productGold;
    [SerializeField] private int productMagic;
    [SerializeField] private int productMaterial;

    protected override void OnStartTurn(BaseKingdom kingdom)
    {
        base.OnStartTurn(kingdom);
        Dictionary<ResourceType, int> product = new Dictionary<ResourceType, int>();
        if (productGold > 0)    product.Add(ResourceType.Gold, productGold);
        if (productMagic > 0)   product.Add(ResourceType.Magic, productMagic);
        if (productMaterial > 0)product.Add(ResourceType.Materials, productMaterial);
        Resource.Instance.AddAll(product);
    }
}