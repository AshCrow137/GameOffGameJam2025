using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAbilityData", menuName = "UnitAbilities/BaseAbilityData")]
public class BaseAbilityData : ScriptableObject
{


    [SerializeField]
    protected AbilityTypeEnum abilityType; public AbilityTypeEnum UnitAbilityType { get { return abilityType; } set { abilityType = value; } }
    [SerializeField]
    protected int cost; public int Cost { get { return cost; } private set { cost = value; } }
    [SerializeField]
    protected AbilityResource abilityResource; public AbilityResource AbilityResource { get { return abilityResource; } private set { abilityResource = value; } }
    [SerializeField]
    protected int castTime; public int CastTime { get { return castTime; } private set { castTime = value; } }

    [SerializeField]
    protected int castDistance; public int CastDistance { get { return castDistance; } private set { castDistance = value; } }
    [SerializeField]
    protected int effectRadius; public int EffectRadius { get { return effectRadius; } private set { effectRadius = value; } }
    [SerializeField]
    protected bool hasTarget; public bool HasTArget { get { return hasTarget; } private set { hasTarget = value; } }
    [SerializeField]
    protected int power; public int Power { get { return power; } private set { power = value; } }
    public virtual UA_Base InstantiateAbility(BaseGridUnitScript ownerUnit)
    {
        return new UA_Base(ownerUnit, abilityType, cost, abilityResource, castTime, castDistance, effectRadius, hasTarget, power);
    }
}
[Serializable]
public class UA_Base
{
    public AbilityTypeEnum AbilityType { get; protected set; }
    public int Cost { get; protected set; }
    public AbilityResource AbilityResource { get; protected set; }
    public int CastTime { get; protected set; }
    public int CastDistance { get; protected set; }
    public int EffectRadius { get; protected set; }
    public bool HasTarget { get; protected set; }
    public int Power { get; protected set; }

    protected BaseGridUnitScript Owner;


    public UA_Base(BaseGridUnitScript owner, AbilityTypeEnum abilityType, int cost, AbilityResource abilityResource, int castTime, int castDistance, int effectRadius, bool hasTarget, int power)
    {
        Owner = owner;
        AbilityType = abilityType;
        Cost = cost;
        AbilityResource = abilityResource;
        CastTime = castTime;
        CastDistance = castDistance;
        EffectRadius = effectRadius;
        HasTarget = hasTarget;
        Power = power;
    }
    public virtual void AmplifyAbility(int amount)
    {
        Power += amount;
    }
    public virtual void ActivateAbility()
    {

    }
    public virtual void ExecuteAbility()
    {

    }
    public virtual void FinishAbility()
    {

    }

}