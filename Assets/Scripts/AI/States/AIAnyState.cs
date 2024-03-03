using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAnyState : AIState, ISpotted
{
    [SerializeField] AIController controller;
    [SerializeField] AIStateManager stateManager;

    Vector3 viewPos;

    bool spottedByTorch = false;
    bool visibleToCamera = false;
    bool _spottedByHighBeam = false;
    public bool spottedByHighBeam => _spottedByHighBeam;
    bool _caught = false;
    public bool caught => _caught;

    [SerializeField] int affectLightRange = 20;
    bool isWithinRange = false;

    [SerializeField] CaughtByBeam caughtByBeam;

    public override void EnterState(AIStateManager state)
    {
        
    }

    public override void UpdateState(AIStateManager state)
    {


        TorchFlicker();
    }

    public override void ExitState(AIStateManager state)
    {
        
    }

    private void Caught(bool isCaught)
    {
        if(isCaught)
        {
            stateManager.SwitchToTheNextState(stateManager.CaughtState);
        }
    }

    

    /*private void CheckIfCaught(AIStateManager state)
    {
        *//*if(visibleToCamera && spottedByTorch && controller.flashLight.beamControl && !spottedByHighBeam)
        {
            _caught = false;
        }*//*
        if (visibleToCamera && spottedByTorch)
        {
            if (controller.flashLight.flashLightSwitch)
            {
                _caught = true;
                state.SwitchToTheNextState(state.CaughtState);
            }
            *//*else if (controller.flashLight.beamControl )
            {

            }*//*  
            else
            {
                _caught = false;
            }
        }
        else
        {
            _caught = false;
        }*//*


        if (controller.flashLight.flashLightSwitch && !controller.flashLight.beamControl)
        {
            if (!caught)
            {
                if (visibleToCamera && spottedByTorch)
                {
                    _caught = true;
                    state.SwitchToTheNextState(state.CaughtState);
                }
            }

            if (!spottedByTorch)
            {
                _caught = false;
            }
        }   
        else if (controller.flashLight.flashLightSwitch && controller.flashLight.beamControl)
        {
            if (!caught)
            {
                if (visibleToCamera && spottedByHighBeam)
                {
                    _caught = true;
                    state.SwitchToTheNextState(state.CaughtState);
                }
            }

            if (!spottedByHighBeam)
            {
                _caught = false;
            }
        }
    }*/

    private void TorchFlicker()
    {

        if (!isWithinRange)
        {
            if (Vector3.Distance(this.transform.position, controller.player.transform.position) < affectLightRange && !caught && !controller.flashLight.beamControl)
            {
                isWithinRange = true;
                controller.flashLight.FlickeringTorch(isWithinRange);
            }
        }
        else
        {
            if (Vector3.Distance(this.transform.position, controller.player.transform.position) > affectLightRange || caught || controller.flashLight.beamControl)
            {
                isWithinRange = false;
                controller.flashLight.FlickeringTorch(isWithinRange);
            }
        }
    }

    /* private void VisibleToCamera()
     {
         viewPos = controller.cam.WorldToViewportPoint(this.transform.position);

         if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
         {
             visibleToCamera = true;
         }
         else
         {
             visibleToCamera = false;
         }
     }*/

    public void SpottedByTorch(bool isSpotted)
    {
        spottedByTorch = isSpotted;
    }

    public void SpottedByHighBeam(bool isSpotted)
    {
        _spottedByHighBeam = isSpotted;
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
        /*if (isSpotted)
        {
            caughtByBeam.caughtEvent.AddListener(Caught);
        }
        else
        {
            caughtByBeam.caughtEvent.RemoveListener(Caught);
        }*/
    }
}
