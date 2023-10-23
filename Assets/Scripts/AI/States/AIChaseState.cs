using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIChaseState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    bool scream = false;

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

        if (GameManager.GameManagerInstance.score > 2)
        {
            controller.anim.SetBool("isScary", true);
            if (transform.right.x > 0)
            {
                controller.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (transform.right.x < 0)
            {
                controller.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (controller.player.GetComponent<PlayerController>().invisible == false) 
        {
            transform.root.position = Vector2.MoveTowards(transform.position, controller.player.transform.position, controller.stats.chaseSpeed * Time.deltaTime);
        }
        else
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        if (Vector3.Distance(this.transform.position, controller.player.transform.position) < 10 && !scream && GameManager.GameManagerInstance.score > 0)
        {
            GetComponentInParent<SoundController>().PlayOneShotSound(0);
            scream = true;
        }
        else if (Vector3.Distance(this.transform.position, controller.player.transform.position) < 10 && !scream && GameManager.GameManagerInstance.score == 0 && !scream)
        {
            GetComponentInParent<SoundController>().PlayOneShotSound(1);
            scream = true;
        }
        else if (Vector3.Distance(this.transform.position, controller.player.transform.position) > 10)
        {
            scream = false;
        }


    }

}
