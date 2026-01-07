
using UnityEngine;
[CreateAssetMenu(fileName = "BaseUnitEffect", menuName = "UnitEffects/BasaeUnitEffect")]
public class BaseUnitEffect:ScriptableObject
{
    public BaseGridUnitScript OwnerUnit {  get; private set; }
    public BaseGridUnitScript TargetUnit { get; private set; }
    public BaseUnitEffect(BaseGridUnitScript ownerUnit)
    {
        OwnerUnit = ownerUnit;
    }

    public virtual void ApplyEffect(BaseGridUnitScript targetUnit)
    {
        TargetUnit = targetUnit;
    }
    public virtual void RemoveEffect()
    {

    }
}