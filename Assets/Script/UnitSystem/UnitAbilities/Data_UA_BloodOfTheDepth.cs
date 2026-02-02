using System.Collections;
using UnityEngine;


public class Data_UA_BloodOfTheDepth : BaseAbilityData
{
    public override UA_Base InstantiateAbility(BaseGridUnitScript ownerUnit)
    {
        return new UA_BloodOfTheDepth(ownerUnit, abilityType, cost, abilityResource, castTime, castDistance, effectRadius, hasTarget, power); ;
    }

}

public class UA_BloodOfTheDepth : UA_Base
{
    public UA_BloodOfTheDepth(BaseGridUnitScript owner, AbilityTypeEnum abilityType, int cost, AbilityResource abilityResource, int castTime, int castDistance, int effectRadius, bool hasTarget, int power) : base(owner, abilityType, cost, abilityResource, castTime, castDistance, effectRadius, hasTarget, power)
    {
    }
}
