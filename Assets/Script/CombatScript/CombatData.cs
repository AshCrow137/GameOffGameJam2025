using System.Collections.Generic;
using UnityEngine;

//Class created to store the Combatant Unit data.
public class CombatData
{
    UnitStats unitAtacker;
    Dictionary<DamageType, float> damageDealtByType;
    DamageType killedTypeDamage;
    bool hasKilled;
    List<string> str_EffectsApplied = new List<string>();
    int lastInteractTurn;

    public CombatData(UnitStats atacker, BaseEffect effect)
    {
        this.unitAtacker = atacker;
        this.damageDealtByType = new Dictionary<DamageType, float>();
        this.hasKilled = false;
        this.lastInteractTurn = TurnManager.instance.currentTurnCount;
        this.str_EffectsApplied = new List<string>();

        AddEffect(this, effect);
    }
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
    public int LastInteractTurn { get => lastInteractTurn; set => lastInteractTurn = value; }
    public List<string> EffectsApplied { get => str_EffectsApplied; set => str_EffectsApplied = value; }
    public DamageType KilledTypeDamage { get => killedTypeDamage; set => killedTypeDamage = value; }
    
    public void RemoveEffect(CombatData data, BaseEffect effect)
    {
        if(data.str_EffectsApplied.Contains(effect.Name))
        {
            data.str_EffectsApplied.Remove(effect.Name);
        }
    }

    public void AddEffect(CombatData data, BaseEffect effect)
    {
        if(!data.str_EffectsApplied.Contains(effect.Name))
        {
            data.str_EffectsApplied.Add(effect.Name);
        }
    }

    public int CountActiveEffectsByAttacker(UnitStats atacked, CombatData atacker)
    {
        List<BaseEffect> activeEffects = new List<BaseEffect>();
        activeEffects = atacked.GetOwner().activeEffects;

        int effectCount = 0;

        for (int i = 0; i < atacker.str_EffectsApplied.Count; i++)
        {
            if(activeEffects.Exists(e=> e.Name == atacker.str_EffectsApplied[i]))
            {
                effectCount++;
            }
        }

        return effectCount;
    }
}
