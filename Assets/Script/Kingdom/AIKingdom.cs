using UnityEngine;
using System.Collections.Generic;
// Ai controlled kingdom class
public class AIKingdom : BaseKingdom
{
    int relationsWithPlayer;
    public KnightUnit DEBUGunit;
    [SerializeField]
    private MadnessData madnessData;
    [SerializeField]
    private int maxMainProductionBuildings = 2; 
    public int GetMaxMainProductionBuildings() { return maxMainProductionBuildings  ; }

    [SerializeField]
    private int maxSecondaryProductionBuilding = 1; 
    public int GetMaxSecondaryProductionBuildings() { return maxSecondaryProductionBuilding; }

    [SerializeField]
    private ResourceType mainResourceType = ResourceType.Gold;
    public ResourceType GetMainResourceType() { return mainResourceType; }

    [SerializeField]
    private List<BaseGridUnitScript> TotalUnlockableUnits = new List<BaseGridUnitScript>();
    public List<BaseGridUnitScript> GetUnlockableUnits()
    {
        return TotalUnlockableUnits;
    }
    public List<BaseGridUnitScript> GetNotUnlockedUnits()
    {
        List<BaseGridUnitScript> temp = new List<BaseGridUnitScript>(TotalUnlockableUnits);
        foreach(BaseGridUnitScript unit in unlockedUnits)
        {
            if(temp.Contains(unit))
            {
                temp.Remove(unit);
            }
        }
        return temp;
    }
    private MadnessDataStruct currentMadnessEffect;
    public override void Initialize()
    {
        //for (int i = 0; i < 50; i++)
        //{
        //    KnightUnit newUnit = Instantiate(DEBUGunit, new Vector3(-20 + i, -37, 0), Quaternion.identity);
        //    AddUnitToKingdom(newUnit);
        //}
        //for (int i = 0; i < 50; i++)
        //{
        //    KnightUnit newUnit = Instantiate(DEBUGunit, new Vector3(-20 + i, -28, 0), Quaternion.identity);
        //    AddUnitToKingdom(newUnit);
        //}
        //for (int i = 0; i < 50; i++)
        //{
        //    KnightUnit newUnit = Instantiate(DEBUGunit, new Vector3(-20 + i, -34, 0), Quaternion.identity);
        //    AddUnitToKingdom(newUnit);
        //}
        //for (int i = 0; i < 50; i++)
        //{
        //    KnightUnit newUnit = Instantiate(DEBUGunit, new Vector3(-20 + i, -30, 0), Quaternion.identity);
        //    AddUnitToKingdom(newUnit);
        //}
        base.Initialize();
    }
    protected override void OnStartTurn(BaseKingdom kingdom)
    {
        base.OnStartTurn(kingdom);
        if (kingdom != this) return;
        Debug.Log($"AI Kingdom StartTurn | {gameObject.name}");
        AIController.Instance.ExecuteTurn(this);
    }
    protected override void OnEndTurn(BaseKingdom kingdom)
    {
        base.OnEndTurn(kingdom);
    }

    public override void IncreaseMadness(int amount)
    {
        base.IncreaseMadness(amount);
        currentMadnessEffect = madnessData.GetMadnessEffects(madnessLevel);
        //TODO update madness system
        //foreach(BaseGridUnitScript unit in controlledUnits)
        //{
        //    unit.ApplyMadnessEffect(currentMadnessEffect);
        //}
        
    }
    public override void DecreaseMadness(int amount)
    {
        base.DecreaseMadness(amount);
        currentMadnessEffect = madnessData.GetMadnessEffects(madnessLevel);
        //TODO update mandess 
        //foreach (BaseGridUnitScript unit in controlledUnits)
        //{
        //    unit.ApplyMadnessEffect(currentMadnessEffect);
        //}
    }
    public MadnessDataStruct GetCurrentMadnessEffect()
    {
        return madnessData.GetMadnessEffects(madnessLevel);
    }
    public bool IsBuildUnit { get;private set; }
    
    public void StartTurn()
    {


    }

    public void EndTurn()
    {
        Debug.Log($"AI Kingdom EndTurn | {gameObject.name}");
        IsBuildUnit = false;
        TurnManager.instance.OnTurnEnd();
    }
}