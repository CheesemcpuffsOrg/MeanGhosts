using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnyState : AIState
{
    [SerializeField] AIController controller;
    [SerializeField] AIStateManager stateManager;

    Vector3 viewPos;

    bool _seenByTorch = false;
    bool _seenByHighBeam = false;
    bool _visibleToCamera = false;
    bool _caught = false;

    [SerializeField] int affectLightRange = 20;
    bool isWithinRange = false;

    public bool caught => _caught;
    public bool seenByHighBeam => _seenByHighBeam;

    public override void EnterState(AIStateManager state)
    {
        
    }

    public override void UpdateState(AIStateManager state)
    {
        CheckIfCaught(state);
    }

    public override void ExitState(AIStateManager state)
    {
        
    }

    private void CheckIfCaught(AIStateManager state)
    {
        if (!isWithinRange)
        {
            if (Vector3.Distance(this.transform.position, controller.player.transform.position) < affectLightRange && !caught)
            {
                isWithinRange = true;
                controller.flashLight.FlickeringTorch(isWithinRange);
            }
        }
        else
        {
            if (Vector3.Distance(this.transform.position, controller.player.transform.position) > affectLightRange || caught)
            {
                isWithinRange = false;
                controller.flashLight.FlickeringTorch(isWithinRange);
            }
        }

        viewPos = controller.cam.WorldToViewportPoint(this.transform.position);

        if (controller.flashLight.flashLightSwitch)
        {
            if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
            {
                _visibleToCamera = true;
            }
            else
            {
                _visibleToCamera = false;
            }

            if (!caught)
            {
                if (_visibleToCamera && _seenByTorch)
                {
                    _caught = true;
                    state.SwitchToTheNextState(state.CaughtState);
                }
            }

            if (!_seenByTorch)
            {
                _caught = false;
            }
        }
        else
        {
            _caught = false;
        }
    }

    public void SpottedByTorch(bool isSpotted)
    {
        _seenByTorch = isSpotted;
    }

    public void SpottedByHighBeam(bool isSpotted)
    {
        _seenByHighBeam = isSpotted;
    }
}
