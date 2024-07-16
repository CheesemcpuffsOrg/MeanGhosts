using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFleeState : AIState
{
    [SerializeField] AIStateController stateManager;
    [SerializeField] AIController controller;

    [SerializeField] float fleeRange = 60;

    GameObject player;
    AIScriptableObject stats;


    public override void EnterState(AIStateController state)
    {
        if (player == null)
        {
            player = controller.player;
        }

        if (stats == null)
        {
            stats = controller.stats;
        }
    }

    public override void UpdateState(AIStateController state)
    {
        if(Vector2.Distance(this.transform.position, player.transform.position) <= fleeRange)
        {
            MoveAwayFromPlayer();
        }
        else
        {
            state.SwitchToTheNextState(state.IdleState);
        }
        
    }

    public override void ExitState(AIStateController state)
    {
        
    }

    void MoveAwayFromPlayer()
    {
        // Calculate the direction vector from the player to the object, keeping it in 3D space
        Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

        // Move the object towards the new target position
        transform.root.position = Vector3.MoveTowards(transform.position, transform.position + directionAwayFromPlayer, stats.fleeSpeed * Time.deltaTime);
    }
}
