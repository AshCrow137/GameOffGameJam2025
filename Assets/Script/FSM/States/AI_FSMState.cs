using UnityEngine;
public abstract class AI_FSMState
{

    protected readonly AI_FSM Fsm;
    public AI_StateFlags Flag { get; protected set; }
    
    public int BaseWeight { get; protected set; }
    
    public float LockTime { get; protected set; }
    public bool isLocked { get; protected set; } 
    
    private readonly int _flagBonusWeight;
    private readonly int _repetitionPenaltyWeight;
    public AI_FSMState(AI_FSM fsm)
    {
        Fsm = fsm;
    }
    protected void RequestTransition(AI_StateFlags nextFlag)
    {
        Fsm.RequestStateChange(nextFlag);
    }
    public abstract void LockState(float time); 
    public virtual void UnlockState()
    {
        if (this.isLocked)
        {
            this.isLocked = false;
        }
    }
    public virtual void Enter() 
    {
        Debug.Log($"Enter {this} state, flag: {Flag}");
    }
    public virtual void Exit()
    {
        Debug.Log($"Exit {this} state, flag: {Flag}");
    }
    public virtual void Update() { }
}
