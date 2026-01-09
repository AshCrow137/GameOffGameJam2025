using System.Collections.Generic;
using UnityEngine;

//Class created to store the Combatant Unit data.
public class CombatData
{
    UnitStats unitAtacker;
    Dictionary<DamageType, float> damageDealtByType;
    bool hasKilled;
    float damageTaken;

    int lastInteractTurn;

    public CombatData(UnitStats unitAtacker, DamageType damageType, float damageDealt, bool hasKilled, float damageTaken, int lastInteractTurn)
    {
        this.unitAtacker = unitAtacker;
        if(damageDealtByType == null)
        {
            damageDealtByType = new Dictionary<DamageType, float>();
        }
        this.damageDealtByType[damageType] += damageDealt;
        this.hasKilled = hasKilled;
        this.damageTaken = damageTaken;
        this.lastInteractTurn = lastInteractTurn;
    }

    public UnitStats UnitAtacker { get => unitAtacker; set => unitAtacker = value; }
    public Dictionary<DamageType, float> DamageDealtByType { get => damageDealtByType; set => damageDealtByType = value; }
    public bool HasKilled { get => hasKilled; set => hasKilled = value; }
    public float DamageTaken { get => damageTaken; set => damageTaken = value; }
    public int LastInteractTurn { get => lastInteractTurn; set => lastInteractTurn = value; }
}
