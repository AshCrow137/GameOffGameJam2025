//using UnityEngine;

//public class TestExperience : MonoBehaviour
//{
//    public UnitStats atacked;

//    public UnitStats attacker1;
//    public UnitStats attacker2;
//    public UnitStats attacker3;
//    public UnitStats attacker4;
//    public UnitStats attacker5;

//    Combat combat;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        combat = new Combat(atacked);

//        combat.AddDamageToAtacker(attacker2, DamageType.Magic, 10, false, EffectType.Negative, 3);
//        combat.AddDamageToAtacker(attacker1, DamageType.Ranged, 10, true, EffectType.Negative, 2);
//        combat.AddDamageToAtacker(attacker3, DamageType.Melee, 10, false, EffectType.Negative, 3);
//        combat.AddDamageToAtacker(attacker4, DamageType.Ranged, 10, false, EffectType.Negative, 4);
//        combat.combatDataList[1].RemoveEffect(combat.combatDataList[1], EffectType.Negative);
//        combat.AddDamageToAtacker(attacker1, DamageType.Ranged, 10, false, EffectType.Negative, 5);
//        combat.AddDamageToAtacker(attacker5, DamageType.Melee, 10, false, EffectType.Negative, 5);

//        combat.UnitKilled(atacked);
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
