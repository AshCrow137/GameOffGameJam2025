using UnityEngine;

/// <summary>
/// ScriptableObject representing an inventory item and its properties.
/// Usage: Create via <b>Assets &gt; Create &gt; Inventory &gt; Item</b> menu in Unity.
/// Configure item type, name, icon, description, stack size, and equipped effect in the inspector.
/// </summary>
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    /// <summary>The category/type of this item (e.g., Helmet, Weapon, Consumable, etc.).</summary>
    public ItemType itemType;

    /// <summary>The display name of the item.</summary>
    public string itemName;

    /// <summary>The sprite icon representing this item in the UI.</summary>
    public Sprite itemIcon;

    /// <summary>Descriptive text explaining the item's properties or effects.</summary>
    public string description;

    /// <summary>Maximum number of this item that can stack in a single slot.</summary>
    public int maxStackSize = 1;

    /// <summary>Data defining the effect this item applies when equipped (optional).</summary>
    // public StatsModifierEffectData equippedEffectData;

    // BaseEffect appliedEffect;
    // public void ApplyEffectOnEquip(BaseGridUnitScript entity)
    // {
    //     if (equippedEffectData != null)
    //     {
    //         appliedEffect = equippedEffectData.InstantiateEffect(entity.GetOwner(), entity);
    //         appliedEffect.ApplyEffect(entity);
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"Item {itemName} has no equipped effect data.");
    //     }
    // }

    // public void RemoveEffectOnEquip()
    // {
    //     appliedEffect?.RemoveEffect();
    // }
}
[CreateAssetMenu(fileName = "NewEquippableInventoryItem", menuName = "Inventory/EquippableItem")]
public class EquippableInventoryItem : InventoryItem
{
    public EffectApplier effectApplier;
    public void OnEquip(BaseGridUnitScript entity)
    {
        effectApplier.ApplyEffect(entity);
    }

    public void OnUnequip(BaseGridUnitScript entity)
    {
        effectApplier.RemoveEffect(entity);
    }
}
[System.Serializable]
public class EffectApplier
{
    public StatsModifierEffectData equippedEffectData;

    public void ApplyEffect(BaseGridUnitScript entity)
    {
        if (equippedEffectData != null)
        {
            var appliedEffect = equippedEffectData.InstantiateEffect(entity.GetOwner(), entity);
            appliedEffect.ApplyEffect(entity);
        }
    }

    public void RemoveEffect(BaseGridUnitScript entity)
    {
        entity.activeEffects.Find(effect => effect.Name == equippedEffectData.GetEffectName())?.RemoveEffect();
    }
}