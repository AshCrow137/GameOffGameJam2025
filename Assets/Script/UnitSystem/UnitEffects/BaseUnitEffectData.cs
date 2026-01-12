
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "BaseUnitEffectData", menuName = "UnitEffects/BasaeUnitEffect")]
public class BaseUnitEffectData:ScriptableObject
{

    /// <summary>
    /// -1 is infinite effect duration
    /// </summary>
    [SerializeField]
    protected int duration = 3;
    [SerializeField]
    protected ProcRate procRate;
    [SerializeField]
    protected EffectType effectType;
    [SerializeField]
    protected bool AffectedByMagicDefence;
    /// <summary>
    /// this variable value will be ignored is AffectedByMagicDefence is false
    /// </summary>
    [SerializeField]
    protected MagicDefenceEffectModifier effectModifier = MagicDefenceEffectModifier.Full;

    public virtual BaseEffect InstantiateEffect(BaseKingdom owner,BaseGridUnitScript target)
    {
        return new BaseEffect(duration, owner, target, procRate, effectType,effectModifier, AffectedByMagicDefence);
    }
}
/// <summary>
/// How often does the effect occur
/// </summary>
public enum ProcRate
{
    Once,
    EveryTurn
}
public enum EffectType
{
    Positive,
    Negative
}
public enum MagicDefenceEffectModifier
{
    
    Full,
    Half,
    Quarter,
    Ignore

}
[Serializable]
public class BaseEffect
{
    public int Duration { get; private set; }
    public ProcRate ProcRate { get; private set; } 
    public BaseKingdom OwnerKingdom {  get; private set; }
    public BaseGridUnitScript TargetUnit { get; private set; }
    public int RemainDuration { get; private set; }
    public EffectType EffectType { get; private set; }

    public bool AffectedByMagicDefence { get; private set; }
    public MagicDefenceEffectModifier EffectModifier { get; private set; }
   
    public BaseEffect(int duration, BaseKingdom ownerKingdom, BaseGridUnitScript targetUnit,ProcRate procRate,EffectType effectType,MagicDefenceEffectModifier effectModifier,bool affectedByMagicDefence)
    {
        
        Duration = duration;
        OwnerKingdom = ownerKingdom;
        TargetUnit = targetUnit;
        ProcRate = procRate;
        EffectType = effectType;
        AffectedByMagicDefence = affectedByMagicDefence;
        EffectModifier = effectModifier;

        switch (effectType)
        {
            case EffectType.Positive:
                break;
            case EffectType.Negative:
                if(AffectedByMagicDefence)
                {
                    RemainDuration = duration - Mathf.RoundToInt(targetUnit.unitStats.UnitMagicDefence.FinalMagicDefence * GetMagicDefenceModifier(effectModifier));
                }
                break;
        }
    }

    private float GetMagicDefenceModifier(MagicDefenceEffectModifier effectModifier)
    {
        switch (effectModifier)
        {
            case MagicDefenceEffectModifier.Full: return 1;
            case MagicDefenceEffectModifier.Half: return 0.5f;
            case MagicDefenceEffectModifier.Quarter: return 0.25f;
            case MagicDefenceEffectModifier.Ignore: return 0;
        }
        return 0;
    }

    public virtual void ApplyEffect(BaseGridUnitScript targetUnit)
    {
        TargetUnit = targetUnit;
        Debug.Log($"aplly {this} effect to {TargetUnit.name}");
        RemainDuration = Duration;
    }
    public virtual void RemoveEffect()
    {
        Debug.Log($"remove {this} effect to {TargetUnit.name}");
    }
    public virtual void DecreaseDuration()
    {
        RemainDuration--;

    }
}