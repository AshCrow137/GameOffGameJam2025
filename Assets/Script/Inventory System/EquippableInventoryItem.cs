using UnityEngine;

[CreateAssetMenu(fileName = "NewEquippableInventoryItem", menuName = "Inventory/EquippableItem")]
public class EquippableInventoryItem : InventoryItem
{
    public EffectApplier effectApplier;
    /// <summary>
    /// Applies the effect associated with this item to the target entity.
    /// </summary>
    /// <param name="entity">The entity equipping the item.</param>
    public void OnEquip(BaseGridUnitScript entity)
    {
        effectApplier.ApplyEffect(entity);
    }

    /// <summary>
    /// Removes the effect associated with this item from the target entity.
    /// </summary>
    /// <param name="entity">The entity unequipping the item.</param>
    public void OnUnequip(BaseGridUnitScript entity)
    {
        effectApplier.RemoveEffect(entity);
    }
}
/// <summary>
/// Helper class to handle applying and removing stats effects.
/// </summary>
[System.Serializable]
public class EffectApplier
{
    public StatsModifierEffectData equippedEffectData;

    public void ApplyEffect(BaseGridUnitScript entity)
    {
        if (equippedEffectData != null)
        {
            var appliedEffect = equippedEffectData.InstantiateEffect(entity.GetOwner(), entity);
            entity.AddEffect(appliedEffect);
        }
    }

    public void RemoveEffect(BaseGridUnitScript entity)
    {
        BaseEffect appliedEffect = entity.activeEffects.Find(effect => effect.Name == equippedEffectData.GetEffectName());
        entity.RemoveEffect(appliedEffect);
    }
}