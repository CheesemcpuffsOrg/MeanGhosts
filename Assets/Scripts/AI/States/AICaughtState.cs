using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {

    }

    public override void UpdateState(AIStateManager state)
    {
        if (!stateManager.caught)
        {
            state.SwitchToTheNextState(state.IdleState);
        }
    }

    public override void ExitState(AIStateManager state)
    { 

    }

    

}
