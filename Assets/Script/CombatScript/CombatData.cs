using System.Collections.Generic;
using UnityEngine;

//Class created to store the Combatant Unit data.
public class CombatData
{
    UnitStats unitAtacker;
    Dictionary<DamageType, float> damageDealtByType;
    bool hasKilled;
    float damageTaken;
    List<EffectsType> effectsApplied = new List<EffectsType>();
    int lastInteractTurn;

    public CombatData(UnitStats unitAtacker, DamageType damageType, float damageDealt, bool hasKilled, float damageTaken, EffectsType effectApply, int lastInteractTurn)
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
        this.damageTaken = damageTaken;
        if (!effectsApplied.Contains(effectApply))
        {
            effectsApplied.Add(effectApply);
        }
        this.lastInteractTurn = lastInteractTurn;
    }

    public UnitStats UnitAtacker { get => unitAtacker; set => unitAtacker = value; }
    public Dictionary<DamageType, float> DamageDealtByType { get => damageDealtByType; set => damageDealtByType = value; }
    public bool HasKilled { get => hasKilled; set => hasKilled = value; }
    public float DamageTaken { get => damageTaken; set => damageTaken = value; }
    public int LastInteractTurn { get => lastInteractTurn; set => lastInteractTurn = value; }
    public List<EffectsType> EffectsApplied { get => effectsApplied; set => effectsApplied = value; }

    public void RemoveEffect(CombatData data, EffectsType effect)
    {
        if(data.effectsApplied.Contains(effect))
        {
            data.effectsApplied.Remove(effect);
        }
    }

    public void AddEffect(CombatData data, EffectsType effect)
    {
        if(!data.effectsApplied.Contains(effect))
        {
            data.effectsApplied.Add(effect);
        }
    }
}
