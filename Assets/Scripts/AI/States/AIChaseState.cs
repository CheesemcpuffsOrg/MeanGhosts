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
            if (!visibleToCamera.IsVisible)
            {
                MoveTowardsPlayer();
            }
        }

        /*if (AIManager.AIManagerInstance.GetCurrentState() != GlobalAIBehaviourState.Timid)
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
            transform.root.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.chaseSpeed * Time.deltaTime);
        }
        else
        {
            state.SwitchToTheNextState(state.IdleState);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 10 && !scream && AIManager.AIManagerInstance.GetCurrentState() != GlobalAIBehaviourState.Timid)
        {
            if (!AudioManager.AudioManagerInstance.IsSoundPlaying(screamSFX, gameObject))
            {
                AudioManager.AudioManagerInstance.PlaySound(screamSFX, gameObject);
                scream = true;
            }          
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 10 && !scream && AIManager.AIManagerInstance.GetCurrentState() == GlobalAIBehaviourState.Timid && !scream)
        {
            if (!AudioManager.AudioManagerInstance.IsSoundPlaying(screamSFX, gameObject))
            {
                AudioManager.AudioManagerInstance.PlaySound(screamSFX, gameObject);
                scream = true;
            }  
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > 10)
        {
            scream = false;
        }*/


    }

    void MoveTowardsPlayer()
    {
        transform.root.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.chaseSpeed * Time.deltaTime);
    }

}
