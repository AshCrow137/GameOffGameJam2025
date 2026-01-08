using UnityEngine;

//Class created to store the Combatant Unit data.
public class CombatData
{
    UnitStats unitAtacker;
    float damageDealt;
    bool hasKilled;
    int lastInteractTurn;

    public CombatData(UnitStats unitAtacker, float damageDealt, bool hasKilled, int lastInteractTurn)
    {
        this.unitAtacker = unitAtacker;
        this.damageDealt = damageDealt;
        this.hasKilled = hasKilled;
        this.lastInteractTurn = lastInteractTurn;
    }

    public UnitStats UnitAtacker { get => unitAtacker; set => unitAtacker = value; }
    public float DamageDealt { get => damageDealt; set => damageDealt = value; }
    public bool HasKilled { get => hasKilled; set => hasKilled = value; }
    public int LastInteractTurn { get => lastInteractTurn; set => lastInteractTurn = value; }
}
