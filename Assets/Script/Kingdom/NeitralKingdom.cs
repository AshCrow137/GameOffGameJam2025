using System.Collections.Generic;
using UnityEngine;

public class NeitralKingdom : BaseKingdom
{
    [SerializeField]
    private List<BaseGridEntity> controlledEntities = new List<BaseGridEntity>();

    public static NeitralKingdom Instance;

    public override void Initialize()
    {
        base.Initialize();
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (var entity in controlledEntities)
        {
            entity?.Initialize(this);
        }
    }
    protected override void DefeatCheck()
    {

    }
}
