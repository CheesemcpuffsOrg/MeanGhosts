using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIController controller;

    bool timeToExplode = false;

    public override void EnterState(AIStateManager state)
    {

    }

    public override void UpdateState(AIStateManager state)
    {
        if (!state.AnyState.caught)
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        if(state.AnyState.seenByHighBeam && controller.flashLight.beamControl) 
        {
            state.SwitchToTheNextState(state.IdleState);
            timeToExplode=true;
        }
    }

    public override void ExitState(AIStateManager state)
    {
        if(timeToExplode)
        {
            transform.root.position = controller.spawn;
            state.AnyState.SpottedByHighBeam(false);
            state.AnyState.SpottedByTorch(false);
            timeToExplode = false;
        }
        
    }

}
