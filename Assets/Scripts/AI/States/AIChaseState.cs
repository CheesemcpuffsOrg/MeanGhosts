using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIChaseState : AIState
{
    [SerializeField] AIStateController stateManager;
    [SerializeField] AIController controller;
    [SerializeField] VisibleToCamera visibleToCamera;

    GameObject player;
    AIScriptableObject stats;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject giggleSFX;
    [SerializeField] AudioScriptableObject screamSFX;

    public override void EnterState(AIStateController state)
    {
        if(player == null)
        {
            player = controller.player;
        }

        if(stats == null)
        {
            stats = controller.stats;
        }
    }

    public override void UpdateState(AIStateController state)
    {
        ChasePlayer(state);
    }

    public override void ExitState(AIStateController state)
    {
        
    }

    void ChasePlayer(AIStateController state)
    {

        if (AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Timid)
        {
            //this makes sure the ghost moves towards the player and backs off when the player gets closer
            if (!visibleToCamera.IsVisible)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveAwayFromPlayer();
            }
        }
        else if (AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Curious || AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Angry)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.root.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.chaseSpeed * Time.deltaTime);
    }

    void MoveAwayFromPlayer()
    {
        // Calculate the direction vector from the player to the object, keeping it in 3D space
        Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

        // Move the object towards the new target position
        transform.root.position = Vector3.MoveTowards(transform.position, transform.position + directionAwayFromPlayer, stats.fleeSpeed * Time.deltaTime);
    }


}
