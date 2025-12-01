using System.Collections.Generic;
using UnityEngine;

public class NeitralKingdom : BaseKingdom
{
   [SerializeField]
   private List<BaseGridEntity> controlledEntities = new List<BaseGridEntity>();


    public override void Initialize()
    {
        base.Initialize();
        foreach (var entity in controlledEntities)
        {
            entity?.Initialize(this);
        }
    }
}
