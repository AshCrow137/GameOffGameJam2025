using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


    public class UnitStats : MonoBehaviour
    {

    [Header("Unit stats")]
    [SerializeField]
    protected StatExp UnitExp;
    [SerializeField]
    protected StatHealth UnitHealth;
    [SerializeField]
    protected StatVision UnitVision;
    [SerializeField]
    protected StatMeleeDamage UnitMeleeDamage;
    [SerializeField]
    protected StatRangedDamage UnitRangedDamage;
    [SerializeField]
    protected StatCounterattack UnitCounterAttack;
    [SerializeField]
    protected StatAttackRange UnitAttackRange;
    [SerializeField]
    protected StatMeleeDefence UnitMeleeDefence;
    [SerializeField]
    protected StatRangedDefence UnitRangedDefence;
    [SerializeField]
    protected StatMagicDefence UnitMagicDefence;
    [SerializeField]
    protected StatMovementDistance UnitMovementDistance;

    [SerializeField]
    protected StatStamina UnitStamina;
    [SerializeField]
    protected StatMana UnitMana;
    [SerializeField]
    protected StatAttacksPerTurn UnitAttacksPerTurn;
    [SerializeField]
    protected int LocalMadness = 0;
    [SerializeField]
    protected int ProductionTime = 1;



    private BaseGridUnitScript owner;

    private List<StatBase> unitStats;

    public void Initialize()
    {
        unitStats = new List<StatBase>() { UnitHealth};
    }
    public T GetUnitStat<T>() where T:StatBase
    {
        foreach(StatBase stat in unitStats)
        {
            if(stat is T cast)
            {
                return cast;
            }
        }
        return null;
    }
    /// <summary>
    /// Changing Madness. + to add; - to remove. Value clamped 0-100
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMadness(int amount)
    {
        LocalMadness += amount;
        LocalMadness = Mathf.Clamp(LocalMadness, 0, 100);
        CheckMadnessValue();
    }
    private void CheckMadnessValue()
    {
        //TODO CHeck and aply madness effects;
    }
}
public enum UnitStatsEnum
{
    Health,
    Vision,
    MeleeDamage,
    RangeDamage,
    Counterattack,
    AttackRange,
    MeleeDefence,
    RangedDefence,
    MagicDefence,
    MovementDistance,
    Stamina,
    Mana,
    AttacksPerTurn
}
[Serializable]
public abstract class StatBase
{
    public virtual void ChangeStat(int amount)
    {

    }
    public virtual void ChangeBaseStat(int amount)
    {

    }
    public virtual void Initialize()
    {

    }
}
[Serializable]
public sealed class StatHealth:StatBase
{
    [SerializeField]
    private int MaxHealth = 10;
    [SerializeField]
    private int CurrentHealth = 10;
    // Actual max health after all effects and modificators
    public int FinalMaxHealth { get; private set; }

    /// <summary>
    /// Changing FinalMaxStamina. Use "+" to add max health; "-" to remove maxHealth
    /// </summary>
    /// <param name="amount"></param>
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMaxHealth += amount;
        if (CurrentHealth > FinalMaxHealth) CurrentHealth = FinalMaxHealth;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MaxHealth += amount;
        FinalMaxHealth += amount;
        if (CurrentHealth > FinalMaxHealth) CurrentHealth = FinalMaxHealth;

    }
    public override void Initialize()
    {
        base.Initialize();
        FinalMaxHealth = MaxHealth;
    }
}
[Serializable]
public sealed class StatExp:StatBase
{
    public int ExpModifier = 35;
    public int ExpToNextLvl = 0;
    public int CurrentExp=0;
    public int Level=0;
    
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        if(CurrentExp >= ExpToNextLvl)
        {
            int r = CurrentExp - ExpToNextLvl;
            CurrentExp = r;
            LvlUp(Level);
        }
    }
    private void LvlUp(int previousLvl)
    {   
        Level++;
        Level = Mathf.Clamp(Level, 0, 99);
    }

}
[Serializable]
public sealed class StatVision:StatBase
{
    public int Vision = 5;
    public int FinalVision { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalVision += amount;
    }
    public override void Initialize()
    {
        base.Initialize();
        FinalVision = Vision;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        Vision += amount;
        FinalVision += amount;
    }
}
[Serializable]
public sealed class StatMeleeDamage : StatBase
{
    public int MeleeDamage = 1;
    public int FinalMeleeDamage { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMeleeDamage += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MeleeDamage += amount;
        FinalMeleeDamage += amount;
    }
    public override void Initialize()
    {
        FinalMeleeDamage = MeleeDamage;
    }
}
[Serializable]
public sealed class StatRangedDamage : StatBase
{
    public int RangedDamage = 1;
    public int FinalRangedDamage { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalRangedDamage += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        RangedDamage += amount;
        FinalRangedDamage += amount;
    }
    public override void Initialize()
    {
        FinalRangedDamage = RangedDamage;
    }
}
[Serializable]
public sealed class StatCounterattack : StatBase
{
    public int Counterattack = 1;
    public int FinalCounterattack { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalCounterattack += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        Counterattack += amount;
        FinalCounterattack += amount;
    }
    public override void Initialize()
    {
        FinalCounterattack = Counterattack;
    }
}
[Serializable]
public sealed class StatMeleeDefence : StatBase
{
    public int MeleeDefence = 1;
    public int FinalMeleeDefence { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMeleeDefence += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MeleeDefence += amount;
        FinalMeleeDefence += amount;
    }
    public override void Initialize()
    {
        FinalMeleeDefence = MeleeDefence;
    }
}
[Serializable]
public sealed class StatRangedDefence : StatBase
{
    public int RangedDefence = 1;
    public int FinalRangedDefence { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalRangedDefence += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        RangedDefence += amount;
        FinalRangedDefence += amount;
    }
    public override void Initialize()
    {
        FinalRangedDefence = RangedDefence;
    }
}
[Serializable]
public sealed class StatMagicDefence : StatBase
{
    public int MagicDefence = 1;
    public int FinalMagicDefence { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMagicDefence += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MagicDefence += amount;
        FinalMagicDefence += amount;
    }
    public override void Initialize()
    {
        FinalMagicDefence = MagicDefence;
    }
}
[Serializable]
public sealed class StatAttackRange : StatBase
{
    public int AttackRange = 1;
    public int FinalAttackRange { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalAttackRange += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        AttackRange += amount;
        FinalAttackRange += amount;
    }
    public override void Initialize()
    {
        FinalAttackRange = AttackRange;
    }
}
[Serializable]
public sealed class StatMovementDistance : StatBase
{
    public int MovementDistance = 1;
    public int FinalMovementDistance { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMovementDistance += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MovementDistance += amount;
        FinalMovementDistance += amount;
    }
    public override void Initialize()
    {
        FinalMovementDistance = MovementDistance;
    }
}
[Serializable]
public sealed class StatAttacksPerTurn : StatBase
{
    public int AttacksPerTurn = 1;
    public int FinalAttacksPerTurn { get; private set; }
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalAttacksPerTurn += amount;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        AttacksPerTurn += amount;
        FinalAttacksPerTurn += amount;
    }
    public override void Initialize()
    {
        FinalAttacksPerTurn = AttacksPerTurn;
    }
}
[Serializable]
public sealed class StatStamina : StatBase
{
    [SerializeField]
    private int MaxStamina = 10;
    [SerializeField]
    private int CurrentStamina = 10;
    // Actual max stamina after all effects and modificators
    public int FinalMaxStamina { get; private set; }

    /// <summary>
    /// Changing FinalMaxStamina. Use "+" to add max stamina; "-" to remove max stamina
    /// </summary>
    /// <param name="amount"></param>
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMaxStamina += amount;
        if (CurrentStamina > FinalMaxStamina) CurrentStamina = FinalMaxStamina;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MaxStamina += amount;
        FinalMaxStamina += amount;
        if (CurrentStamina > FinalMaxStamina) CurrentStamina = FinalMaxStamina;

    }
    public override void Initialize()
    {
        base.Initialize();
        FinalMaxStamina = MaxStamina;
    }
}
[Serializable]
public sealed class StatMana : StatBase
{
    [SerializeField]
    private int MaxMana = 10;
    [SerializeField]
    private int CurrentMana = 10;
    // Actual max mana after all effects and modificators
    public int FinalMaxMana { get; private set; }

    /// <summary>
    /// Changing FinalMaxMana. Use "+" to add max mana; "-" to remove max mana
    /// </summary>
    /// <param name="amount"></param>
    public override void ChangeStat(int amount)
    {
        base.ChangeStat(amount);
        FinalMaxMana += amount;
        if (CurrentMana > FinalMaxMana) CurrentMana = FinalMaxMana;
    }
    public override void ChangeBaseStat(int amount)
    {
        base.ChangeBaseStat(amount);
        MaxMana += amount;
        FinalMaxMana += amount;
        if (CurrentMana > FinalMaxMana) CurrentMana = FinalMaxMana;

    }
    public override void Initialize()
    {
        base.Initialize();
        FinalMaxMana = MaxMana;
    }
}

