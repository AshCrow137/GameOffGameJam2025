

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsModifierEffect", menuName = "UnitEffects/StatsModifierEffect")]
public class StatsModifierEffect : BaseUnitEffect
{
    public List<StatToModifyStruct> StatsToModify = new List<StatToModifyStruct>();
    public StatsModifierEffect(BaseGridUnitScript ownerUnit,List<StatToModifyStruct> statsToModify) : base(ownerUnit)
    {
        StatsToModify = statsToModify;
    }
    public override void ApplyEffect(BaseGridUnitScript targetUnit)
    {
        base.ApplyEffect(targetUnit);

    }

}
[Serializable]
public struct StatToModifyStruct
{
    public UnitStatsEnum StatName;
    public int Value;
}
