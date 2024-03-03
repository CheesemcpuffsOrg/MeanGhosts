using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICaughtState : AIState
{

    [SerializeField] AIController controller;
    [SerializeField] AIStateManager stateManager;

    bool highBeamOnGhost = false;

    [SerializeField] float timeUntilExplode = 2;

    Coroutine caughtByBeam;

    [SerializeField]ParticleSystem poofPA;

    [SerializeField] CaughtByBeam CaughtByBeam;

    private void Start()
    {
        CaughtByBeam.caughtEvent.AddListener(Freed);
    }

    public override void EnterState(AIStateManager state)
    {
        controller.flashLight.highBeamIsActive.AddListener(SpottedByActiveHighBeam);
    }

    public override void UpdateState(AIStateManager state)
    {
        /*if (!state.AnyState.caught)
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        if (!state.AnyState.spottedByHighBeam || !controller.flashLight.flashLightSwitch || !controller.flashLight.beamControl)
        {
            if (caughtByBeam != null)
            {
                StopCoroutine(caughtByBeam);
                highBeamOnGhost = false;
            }
        }*/
    }

    public override void ExitState(AIStateManager state)
    {
        if(highBeamOnGhost)
        {
            var poof = Instantiate(poofPA, this.transform.position, this.transform.rotation);
            poof.Play();
            transform.root.position = controller.spawn.position;
            state.AnyState.SpottedByHighBeamInterface(false);
            state.AnyState.SpottedByTorch(false);
            highBeamOnGhost = false;
            controller.flashLight.highBeamIsActive.RemoveListener(SpottedByActiveHighBeam);
        }
    }

    private void Freed(bool isCaught)
    {
        if (!isCaught)
        {
            stateManager.SwitchToTheNextState(stateManager.IdleState);
        }
    }

    void SpottedByActiveHighBeam()
    {
        //Debug.Log("spotted");

        if (stateManager.AnyState.spottedByHighBeam)
        {
            highBeamOnGhost = true;
            caughtByBeam = StartCoroutine(DeathTimer());
        }
    }

    IEnumerator DeathTimer()
    {
        //Debug.Log("Start timer");

        yield return new WaitForSeconds(timeUntilExplode);

        stateManager.SwitchToTheNextState(stateManager.IdleState);  
    }

}
