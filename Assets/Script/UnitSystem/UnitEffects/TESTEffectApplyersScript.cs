using System.Collections.Generic;
using UnityEngine;

public class TESTEffectApplyersScript : MonoBehaviour
{
    public BaseUnitEffectData effectData;
    public List<BaseGridUnitScript> testUnits = new List<BaseGridUnitScript>();
    private void Awake()
    {
        foreach (BaseGridUnitScript unit in testUnits)
        {
            unit.MovementFinishEvent.AddListener(CheckIfUnitOnTile);
        }
        
    }
    private void CheckIfUnitOnTile()
    {
        foreach (BaseGridUnitScript unit in testUnits)
        {
            if (unit.GetCellPosition() == HexTilemapManager.Instance.WorldToCellPos(transform.position))
            { 
                unit.AddEffect(effectData.InstantiateEffect(NeitralKingdom.Instance,unit));
            }
        }
        
    }
}
