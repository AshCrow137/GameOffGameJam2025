using System;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance { get; private set; }
    
    [SerializeField] private GameObject baseMeleeUnitPrefab; //need for call spawned func

    Dictionary<ResourceType, int> unitRequireCost = new Dictionary<ResourceType, int>
    {
        {ResourceType.Gold, 5}
    };
    
    Dictionary<ResourceType, int> buildingsRequireCost = new Dictionary<ResourceType, int>
    {
        {ResourceType.Gold, 10},
        {ResourceType.Materials , 5}
    };
    
    public void Initialize()
    {
        Instance = this;
    }
    
    /// <summary>
    /// Make turn for current bot
    /// </summary>
    /// <param name="kingdom"></param>
    public bool ExecuteTurn(AIKingdom kingdom)
    {
        while (GetPriorityAction(kingdom) != AIAction.None)
        {
            AIAction action = GetPriorityAction(kingdom);
            switch (action)
            {
                case AIAction.None:
                    break;
                case AIAction.Attack:
                    Debug.Log($"AI Choose [{kingdom.gameObject.name}] attack");
                    break;
                case AIAction.PreferredAttack:
                    Debug.Log($"AI Choose [{kingdom.gameObject.name}] attack preferred target");
                    break;
                case AIAction.BuildCity:
                    Debug.Log($"AI Choose [{kingdom.gameObject.name}] build city");
                    break;
                case AIAction.BuildUnit:
                    GameplayCanvasManager.instance.TryToSpawnUnit(kingdom.GetMainCity(), baseMeleeUnitPrefab);
                    Debug.Log($"AI Choose [{kingdom.gameObject.name}] build unity");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return true;
    }

    private bool CheckResources(AIKingdom kingdom, Dictionary<ResourceType, int> required)
    {
        if (kingdom.Resources().HasEnough(required) != null) // seem to be right, right?
        {
            kingdom.Resources().SpendResource(required);
            return true;
        }
        return false;
    }

    private AIAction GetPriorityAction(AIKingdom kingdom)
    {
        if (kingdom.GetMadnessEffects() == MadnessEffect.almostDefeat)
        {
            return AIAction.None;
        }

        if (CheckResources(kingdom, unitRequireCost) && !kingdom.IsBuildUnit)
        {
            return AIAction.BuildUnit;
        }

        if (CheckResources(kingdom, buildingsRequireCost))
        {
            return AIAction.BuildCity;
        }

        if(CheckPlayerUnits(kingdom, null)) //add later
        {
            switch (GetAttackByMadnessLevel(kingdom))
            {
                case AIAttackType.None:
                    break;
                case AIAttackType.Attack:
                    break;
                case AIAttackType.AttackMiddleMadness:
                    break;
                case AIAttackType.AttackHighMadness:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //if(unit.ShowPlayerSpecialUnit)
        //{
        //  return AIAction.PreferredAttack;
        //}
        
    
    return AIAction.None;
    }

    private bool CheckPlayerUnits(AIKingdom kingdom, PlayerKingdom playerKingdom)
    {
        if (kingdom.ControlledUnits[0])
        {
            //add check distance to player units, AI know all

            
            return true;
        }
        return false;
    }

    private AIAttackType GetAttackByMadnessLevel(AIKingdom kingdom)
    {
        if (kingdom.GetMadnessLevel() >= 0 && kingdom.GetMadnessLevel() <= 30) //setup this values from some ScriptableObject library of values
        {
            return AIAttackType.Attack;
        }
            
        if (kingdom.GetMadnessLevel() >= 31 && kingdom.GetMadnessLevel() <= 70)
        {
            return AIAttackType.AttackMiddleMadness;   
        }
            
        if (kingdom.GetMadnessLevel() >= 71 && kingdom.GetMadnessLevel() <= 99)
        {
            return AIAttackType.AttackHighMadness;
        }
        return AIAttackType.None;
    }
    
}
