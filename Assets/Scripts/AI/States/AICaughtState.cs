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

    Coroutine caughtByBeamCoroutine;

    [SerializeField]ParticleSystem poofPA;

    public override void EnterState(AIStateManager state)
    {
        caughtByBeam.caughtByHighBeamEvent.AddListener(HighBeamControl);
        caughtByBeam.caughtEvent.AddListener(Freed);

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

        if (highBeamOnGhost)
        {
            
           // controller.flashLight.highBeamIsActive.RemoveListener(SpottedByActiveHighBeam);
        }
    }

    private void Freed(bool isCaught)
    {
        if (!isCaught)
        {
            stateManager.SwitchToTheNextState(stateManager.IdleState);
        }
    }

    //need to fix this
    private void HighBeamControl(bool result)
    {
        if (result)
        {
            //highBeamOnGhost = true;
            caughtByBeamCoroutine = StartCoroutine(DeathTimer());
        }
        else if(!result) 
        {
            //highBeamOnGhost = false;
            Debug.Log("not caught by highbeam");
            if(caughtByBeamCoroutine != null)
            {
                StopCoroutine(caughtByBeamCoroutine);
            }

        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeUntilExplode);

        var poof = Instantiate(poofPA, this.transform.position, this.transform.rotation);
        poof.Play();
        transform.root.position = controller.spawn.position;
        caughtByBeam.SpottedByHighBeam(false);
        caughtByBeam.SpottedByTorch(false);
        highBeamOnGhost = false;

        stateManager.SwitchToTheNextState(stateManager.IdleState);  
    }

}
