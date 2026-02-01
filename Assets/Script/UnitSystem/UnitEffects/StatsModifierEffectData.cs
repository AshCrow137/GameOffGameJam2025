

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsModifierEffectData", menuName = "UnitEffects/StatsModifierEffectData")]
public class StatsModifierEffectData : BaseUnitEffectData
{
    //[SerializeReference]
    [SerializeField]
    protected List<StatToModifyTypeAndValue> StatsToModify;

    public override BaseEffect InstantiateEffect(BaseKingdom owner, BaseGridUnitScript target)
    {
        return new StatsModifierEffect(effectName, duration, owner, target, procRate, effectType, effectModifier, AffectedByMagicDefence, StatsToModify);
    }

}
[Serializable]
public class StatsModifierEffect : BaseEffect
{

    public List<StatToModifyTypeAndValue> StatsToModify { get; private set; }
    public StatsModifierEffect(string effectName, int duration, BaseKingdom OwnerKingdom, BaseGridUnitScript targetUnit, ProcRate procRate, EffectType effectType, MagicDefenceEffectModifier effectModifier, bool affectedByMagicDefence, List<StatToModifyTypeAndValue> statsToModify) : base(effectName, duration, OwnerKingdom, targetUnit, procRate, effectType, effectModifier, affectedByMagicDefence)
    {
        StatsToModify = statsToModify;
    }
    public override void ApplyEffect(BaseGridUnitScript targetUnit)
    {
        base.ApplyEffect(targetUnit);
        foreach (var stat in StatsToModify)
        {
            ModifyStat(stat, 1);
        }
    }
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        foreach (var stat in StatsToModify)
        {
            ModifyStat(stat, -1);
        }
    }
    /// <summary>
    /// Use mod = 1 to apply changes to stat, use mod = -1 to reverse changes in stat
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="mod"></param>
    private void ModifyStat(StatToModifyTypeAndValue stat, int mod)
    {
        Debug.Log(stat.Type);
        StatBase statToModify = TargetUnit.unitStats.GetUnitStat(stat.Type);
        if (statToModify != null)
        {
            statToModify.ChangeStat(stat.Value * mod);
        }
    }
}

[Serializable]
public class StatToModifyTypeAndValue
{
    //[SerializeField]
    public InspectableType<StatBase> Type = typeof(StatBase);
    public int Value;
}



[Serializable]
public class InspectableType<T> : ISerializationCallbackReceiver
{

    [SerializeField] string qualifiedName;

    public System.Type storedType { get; private set; }

#if UNITY_EDITOR
    // HACK: I wasn't able to find the base type from the SerializedProperty,
    // so I'm smuggling it in via an extra string stored only in-editor.
    [SerializeField] string baseTypeName;
#endif

    public InspectableType(System.Type typeToStore)
    {
        storedType = typeToStore;
    }

    public override string ToString()
    {
        if (storedType == null) return string.Empty;
        return storedType.Name;
    }

    public void OnBeforeSerialize()
    {
        if (storedType != null)
        {
            qualifiedName = storedType.AssemblyQualifiedName;
        }


#if UNITY_EDITOR
        baseTypeName = typeof(T).AssemblyQualifiedName;
#endif
    }

    public void OnAfterDeserialize()
    {
        if (string.IsNullOrEmpty(qualifiedName) || qualifiedName == "null")
        {
            storedType = null;
            return;
        }
        storedType = System.Type.GetType(qualifiedName);
    }

    public static implicit operator System.Type(InspectableType<T> t) => t.storedType;

    // TODO: Validate that t is a subtype of T?
    public static implicit operator InspectableType<T>(System.Type t) => new InspectableType<T>(t);
}
