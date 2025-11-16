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

        //if(unit.ShowPlayerUnit)
        //{
        //  return AIAction.Attack;
        //}

        //if(unit.ShowPlayerSpecialUnit)
        //{
        //  return AIAction.PreferredAttack;
        //}
        
    
    return AIAction.None;
    }
    
}
