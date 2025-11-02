using UnityEngine;
public abstract class AI_FSMSBossState:AI_FSMState
{

    protected readonly AI_FSM Fsm;

    public AI_FSMSBossState(AI_FSM fsm):base(fsm)
    {
        Fsm = fsm;
    }
    public virtual void Enter() 
    {
        //Debug.Log($"Enter {this} state");
    }
    public virtual void Exit()
    {
        //Debug.Log($"Exit {this} state");
    }
    public virtual void Update() { }
}
