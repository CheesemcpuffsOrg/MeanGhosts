using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;
    [SerializeField] AIAnyState anyState;

    public override void EnterState(AIStateManager state)
    {

    }

    public override void UpdateState(AIStateManager state)
    {
        if (!anyState.caught)
        {
            state.SwitchToTheNextState(state.IdleState);
        }
    }

    public override void ExitState(AIStateManager state)
    { 

    }

    

}
