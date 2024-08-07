using System.Collections;
using UnityEngine;

public class AICaughtState : AIState, ISpotted
{

    [SerializeField] AIController controller;
    [SerializeField] AIStateController stateManager;

    [SerializeField] float timeUntilExplode = 2;

    Coroutine caughtByBeamCoroutine;

    GameObject player;
    AIScriptableObject stats;

    FlashLightState flashLightState;
    bool caughtByTorch;
    bool caughtByHighBeam;

    [SerializeField]ParticleSystem poofPA;

    public override void EnterState(AIStateController state)
    {

        if (AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Timid || AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Curious)
        {
            stateManager.SwitchToTheNextState(stateManager.FleeState);
        }

        if (player == null)
        {
            player = controller.player;
        }

        if (stats == null)
        {
            stats = controller.stats;
        }

        if (flashLightState == FlashLightState.HIGHBEAM)
        {
            caughtByBeamCoroutine = StartCoroutine(DeathTimer());
        }
    }

    public override void UpdateState(AIStateController state)
    {
        if(AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Aggresive && caughtByBeamCoroutine == null)
        {
            MoveTowardsPlayer();   
        }

        CheckFlashLight();
    }

    public override void ExitState(AIStateController state)
    {
        if (caughtByBeamCoroutine != null)
        {
            StopCoroutine(caughtByBeamCoroutine);
            caughtByBeamCoroutine = null;
        }
    }

    private void CheckFlashLight()
    {
        if (flashLightState == FlashLightState.OFF || flashLightState == FlashLightState.COOLDOWN || !caughtByTorch || (!caughtByHighBeam && flashLightState == FlashLightState.HIGHBEAM))
        {
            stateManager.SwitchToTheNextState(stateManager.IdleState);
            
        }
        else if (caughtByHighBeam && flashLightState == FlashLightState.HIGHBEAM && caughtByBeamCoroutine == null)
        {
            caughtByBeamCoroutine = StartCoroutine(DeathTimer());
        }
        else if ((!caughtByHighBeam || flashLightState != FlashLightState.HIGHBEAM) && caughtByBeamCoroutine != null)
        {
            StopCoroutine(caughtByBeamCoroutine);
            caughtByBeamCoroutine = null;
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeUntilExplode);

        var poof = Instantiate(poofPA, this.transform.position, this.transform.rotation);
        poof.Play();
        transform.root.position = controller.spawn.position;
        
        controller.flashLight.HasGhostBeenCaught(false);
        controller.flashLight.IsGhostWithinRange(false);

        caughtByBeamCoroutine = null;

        stateManager.SwitchToTheNextState(stateManager.IdleState);  
    }

    void MoveTowardsPlayer()
    {
        transform.root.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.reducedSpeed * Time.deltaTime);
    }

    public void SpottedByTorch()
    {
        caughtByTorch = true;
    }

    public void NotSpottedByTorch()
    {
        caughtByTorch = false;
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
