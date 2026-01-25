using UnityEngine;

[CreateAssetMenu(fileName = "BaseGameplayEvent", menuName = "BaseGameplayEvent")]
public class BaseGameplayEvent : ScriptableObject
{
    [SerializeField]
    protected int EventChance = 5;
    public virtual void ExecuteEvent(BaseKingdom kingdom)
    {

    }

    public int GetEventChance() => EventChance;
}









