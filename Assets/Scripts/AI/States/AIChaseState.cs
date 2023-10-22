using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIChaseState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    public override void EnterState(AIStateManager state)
    {
       
    }

    public override void UpdateState(AIStateManager state)
    {
        ChasePlayer(state);
    }

    public override void ExitState(AIStateManager state)
    {
        
    }

    void ChasePlayer(AIStateManager state)
    {
        if(controller.player.GetComponent<PlayerController>().invisible == false) 
        {
            transform.root.position = Vector2.MoveTowards(transform.position, controller.player.transform.position, controller.stats.chaseSpeed * Time.deltaTime);
        }
        else
        {
            state.SwitchToTheNextState(state.IdleState);
        }
        
    }

}
