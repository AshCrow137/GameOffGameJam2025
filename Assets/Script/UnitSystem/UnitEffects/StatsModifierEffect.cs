

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsModifierEffect", menuName = "UnitEffects/StatsModifierEffect")]
public class StatsModifierEffect : BaseUnitEffect
{
    public List<StatBase> StatsToModify = new List<StatBase>();
    public StatsModifierEffect(BaseGridUnitScript ownerUnit,List<StatBase> statsToModify) : base(ownerUnit)
    {
        StatsToModify = statsToModify;
    }
    public override void ApplyEffect(BaseGridUnitScript targetUnit)
    {
        base.ApplyEffect(targetUnit);

    }

}

