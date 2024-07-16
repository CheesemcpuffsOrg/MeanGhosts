using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAnyState : AIState, ISpotted
{
    [SerializeField] AIController controller;
    [SerializeField] AIStateController stateManager;
    [SerializeField] VisibleToCamera visibleToCamera;

    bool caughtByTorch;
    bool caughtByHighBeam;

    FlashLightState flashLightState;

    public override void EnterState(AIStateController state)
    {

    }

    public override void UpdateState(AIStateController state)
    {
        CheckIfCaught();
    }


    public override void ExitState(AIStateController state)
    {
        
    }

    private void CheckIfCaught()
    {
        if (caughtByTorch && visibleToCamera.IsVisible && (flashLightState == FlashLightState.ON || flashLightState == FlashLightState.FLICKER))
        {
            if (stateManager.CurrentState != stateManager.CaughtState && stateManager.CurrentState != stateManager.FleeState)
            {
                stateManager.SwitchToTheNextState(stateManager.CaughtState);
            }
        }
        else if (caughtByHighBeam && visibleToCamera.IsVisible && flashLightState == FlashLightState.HIGHBEAM)
        {
            if (stateManager.CurrentState != stateManager.CaughtState && stateManager.CurrentState != stateManager.FleeState)
            {
                stateManager.SwitchToTheNextState(stateManager.CaughtState);
            }
        }
    }

    public void SpottedByTorch()
    {
        caughtByTorch = true;
        controller.flashLight.HasGhostBeenCaught(true);
    }

    public void NotSpottedByTorch()
    {
        caughtByTorch = false;
        controller.flashLight.HasGhostBeenCaught(false);
    }

    public void SpottedByHighBeam()
    {
        caughtByHighBeam = true;
    }

    public void NotSpottedByHighBeam()
    {
        caughtByHighBeam = false;
    }

    public void StateOfFlashLight(FlashLightState state)
    {
        flashLightState = state;
    }
}
