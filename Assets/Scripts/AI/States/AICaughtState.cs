using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIController controller;
    [SerializeField] AIStateManager stateManager;
    [SerializeField] CaughtByBeam caughtByBeam;

    bool highBeamOnGhost = false;

    [SerializeField] float timeUntilExplode = 2;
    [SerializeField] float transitionDelay = 1;

    Coroutine caughtByBeamCoroutine;
    Coroutine transitionDelayCoroutine;

    [SerializeField]ParticleSystem poofPA;

    public override void EnterState(AIStateManager state)
    {
        caughtByBeam.caughtByHighBeamEvent.AddListener(HighBeamControl);
        caughtByBeam.caughtEvent.AddListener(Freed);

        controller.flashLight.GhostHasBeenCaught(true);

        if (transitionDelayCoroutine != null)
        {
            StopCoroutine(transitionDelayCoroutine);
        }

        if (controller.flashLight.flashLightState == FlashLight.FlashLightState.HIGHBEAM)
        {

        }
    }

    public override void UpdateState(AIStateManager state)
    {

    }

    public override void ExitState(AIStateManager state)
    {
        

        caughtByBeam.caughtByHighBeamEvent.RemoveListener(HighBeamControl);
        caughtByBeam.caughtEvent.RemoveListener(Freed);

        
    }

    private void Freed(bool isCaught)
    {
        if (!isCaught)
        {
            controller.flashLight.GhostHasBeenCaught(false);

            stateManager.SwitchToTheNextState(stateManager.IdleState);

        }
    }

    //need to fix this
    private void HighBeamControl(bool result)
    {
        if (result)
        {
            caughtByBeamCoroutine = StartCoroutine(DeathTimer());
        }
        else if(!result) 
        {
            if(caughtByBeamCoroutine != null)
            {
                StopCoroutine(caughtByBeamCoroutine);
            }

        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeUntilExplode);

        controller.flashLight.IsGhostWithinRange(false);

        var poof = Instantiate(poofPA, this.transform.position, this.transform.rotation);
        poof.Play();
        transform.root.position = controller.spawn.position;
        controller.flashLight.GhostHasBeenCaught(false);

        stateManager.SwitchToTheNextState(stateManager.IdleState);  
    }
 
}
