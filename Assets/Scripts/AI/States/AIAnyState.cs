using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAnyState : AIState, ISpotted
{
    [SerializeField] AIController controller;
    [SerializeField] AIStateManager stateManager;
    [SerializeField] CaughtByBeam caughtByBeam;

    public override void EnterState(AIStateManager state)
    {

    }

    public override void UpdateState(AIStateManager state)
    {

    }

    public override void ExitState(AIStateManager state)
    {
        
    }

    private void Caught(bool isCaught)
    {
        if(isCaught)
        {
            if(stateManager.CurrentState != stateManager.CaughtState)
            {
                stateManager.SwitchToTheNextState(stateManager.CaughtState);
            }
            
        }
    }

    public void SpottedByTorchInterface(bool isSpotted)
    {
        if (isSpotted)
        {
            caughtByBeam.caughtEvent.AddListener(Caught);
        }
        else
        {
            caughtByBeam.caughtEvent.RemoveListener(Caught);
        }
    }

    public void SpottedByHighBeamInterface(bool isSpotted)
    {
        if (isSpotted)
        {
            caughtByBeam.caughtByHighBeamEvent.AddListener(Caught);
        }
        else
        {
            caughtByBeam.caughtByHighBeamEvent.RemoveListener(Caught);
        }
    }
}
