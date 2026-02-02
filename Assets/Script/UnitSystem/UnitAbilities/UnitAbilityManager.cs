
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityManager : MonoBehaviour
{
    [SerializeField]
    protected List<BaseAbilityData> abilityDataList = new List<BaseAbilityData>(); public List<BaseAbilityData> AbilityDataList { get { return abilityDataList; } set { abilityDataList = value; } }
    protected BaseGridUnitScript Owner;
    protected UnitStats OwnerStats;
    public List<UA_Base> UnitAbilityList = new List<UA_Base>();
    public void Initialize(BaseGridUnitScript owner,UnitStats unitStats)
    {
        Owner = owner;
        OwnerStats = unitStats;
    }
}
