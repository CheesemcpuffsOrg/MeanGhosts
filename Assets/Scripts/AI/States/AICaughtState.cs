using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIController controller;

    bool highBeamOnGhost = false;

    [SerializeField] float timeUntilExplode = 2;

    public override void EnterState(AIStateManager state)
    {

    }

    public override void UpdateState(AIStateManager state)
    {
        if (!state.AnyState.caught)
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        //add delay period until ghost is detroyed
        if(state.AnyState.seenByHighBeam && controller.flashLight.beamControl) 
        {
            highBeamOnGhost = true;
            state.SwitchToTheNextState(state.IdleState);
            StartCoroutine(CaughtByBeam());
            
        }
    }

    public override void ExitState(AIStateManager state)
    {
        if(highBeamOnGhost)
        {
            transform.root.position = controller.spawn.position;
            state.AnyState.SpottedByHighBeam(false);
            state.AnyState.SpottedByTorch(false);
            highBeamOnGhost = false;
        }
        
    }

    IEnumerator CaughtByBeam()
    {
        yield return new WaitForSeconds(timeUntilExplode);
    }

}
