using System.Collections.Generic;
using UnityEngine;

//Class created to store the Combatant Unit data.
public class CombatData
{
    UnitStats unitAtacker;
    Dictionary<DamageType, float> damageDealtByType;
    DamageType killedTypeDamage;
    bool hasKilled;
    float damageTaken;
    List<EffectType> effectsApplied = new List<EffectType>();
    int lastInteractTurn;

    public CombatData(UnitStats unitAtacker, DamageType damageType, float damageDealt, bool hasKilled, int lastInteractTurn)
    {
        this.unitAtacker = unitAtacker;
        if(damageDealtByType == null)
        {
            Debug.Log("Creating new damageDealtByType dictionary.");
            damageDealtByType = new Dictionary<DamageType, float>();
        }
        if(!damageDealtByType.ContainsKey(damageType))
        {
            damageDealtByType[damageType] = 0;
        }
        this.damageDealtByType[damageType] += damageDealt;
        this.hasKilled = hasKilled;
        if (this.hasKilled)
        {
            this.killedTypeDamage = damageType;
        }
        this.lastInteractTurn = lastInteractTurn;
        Debug.Log("CombatData created for attacker: " + unitAtacker.name);
    }

    public UnitStats UnitAtacker { get => unitAtacker; set => unitAtacker = value; }
    public Dictionary<DamageType, float> DamageDealtByType { get => damageDealtByType; set => damageDealtByType = value; }
    public bool HasKilled { get => hasKilled; set => hasKilled = value; }
    public float DamageTaken { get => damageTaken; set => damageTaken = value; }
    public int LastInteractTurn { get => lastInteractTurn; set => lastInteractTurn = value; }
    public List<EffectType> EffectsApplied { get => effectsApplied; set => effectsApplied = value; }
    public DamageType KilledTypeDamage { get => killedTypeDamage; set => killedTypeDamage = value; }
    
    public void RemoveEffect(CombatData data, EffectType effect)
    {
        if(data.effectsApplied.Contains(effect))
        {
            data.effectsApplied.Remove(effect);
        }
    }

    public void AddEffect(CombatData data, EffectType effect)
    {
        if(!data.effectsApplied.Contains(effect))
        {
            data.effectsApplied.Add(effect);
        }
    }
}
