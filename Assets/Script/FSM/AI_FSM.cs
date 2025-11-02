using System;
using System.Collections.Generic;
using System.Linq;

public class AI_FSM
{

    public AI_FSMState currentState { get; private set; }
    
    protected List<AI_FSMState> States = new List<AI_FSMState>();
    private static Random _random = new Random();
    private readonly int _flagBonusWeight;
    private readonly int _repetitionPenaltyWeight;
    public void AddState(AI_FSMState newState)
    {
        this.States.Add(newState);

    }

    protected virtual void ChangeState(AI_FSMState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ForceChangeState(AI_StateFlags newStateFlag)
    {
        //TODO check if state unlocked
        ChangeState(GetStateByFlag(newStateFlag));
    }
    public AI_FSMState GetCurrentState()
    {
        return currentState;
    }
    public void Update()
    {
        currentState?.Update();
    }
    public void RequestStateChange(AI_StateFlags requestedFlag)
    {


        var weightedPool = new List<WeightedStateEntry>();
        int totalWeight = 0;
        AI_StateFlags currentFlags = currentState?.Flag ?? AI_StateFlags.None;

        foreach (var state in States)
        {
            if (state == currentState) continue;

            int finalWeight = state.BaseWeight;
            
            if (state.Flag.HasFlag(requestedFlag))
            {
                finalWeight += _flagBonusWeight;
            }

            if (currentFlags != AI_StateFlags.None && (state.Flag & currentFlags) != 0)
            {
                finalWeight -= _repetitionPenaltyWeight;
            }

            finalWeight = Math.Max(0, finalWeight);

            if (finalWeight > 0)
            {
                totalWeight += finalWeight;
                weightedPool.Add(new WeightedStateEntry(state, totalWeight));
            }
        }


        if (weightedPool.Count == 0)
        {
            if (currentState?.Flag != AI_StateFlags.Basic)
                ChangeState(GetStateByFlag(AI_StateFlags.Basic));
            return;
        }

        int roll = _random.Next(0, totalWeight);

        AI_FSMState nextState = null;
        foreach (var entry in weightedPool)
        {
            if (entry.CumulativeWeight > roll)
            {
                nextState = entry.State;
                break;
            }
        }

        ChangeState(nextState ?? GetStateByFlag(AI_StateFlags.Basic));
    }
    protected AI_FSMState GetStateByFlag(AI_StateFlags flag)
    {
        return States.FirstOrDefault(state => state.Flag.HasFlag(flag));
    }
    private struct WeightedStateEntry
    {
        public AI_FSMState State;
        public int CumulativeWeight; 

        public WeightedStateEntry(AI_FSMState state, int cumulativeWeight)
        {
            State = state;
            CumulativeWeight = cumulativeWeight;
        }
    }
}
