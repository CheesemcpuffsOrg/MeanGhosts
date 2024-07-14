using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIIdleState : AIState
{
    [SerializeField] AIStateController stateManager;
    [SerializeField] AIController controller;
    [SerializeField] AIAnyState anyState;

    Vector2 wanderZone = new Vector2(0,0);
    [SerializeField] Vector2 destPoint;
    //Vector2 finalDestPoint;

    [SerializeField]bool walkPointSet = false;

    [SerializeField]float agroRangeAdjusted;

    public override void EnterState(AIStateController state)
    {
        controller.anim.SetBool("isScary", false);

        destPoint = this.transform.position;
    }

    public override void UpdateState(AIStateController state)
    {
        IdleMovement();

        /* if (!anyState.caught)
         {
             SwitchToChaseState(state);
         } */

        SwitchToChaseState(state);
    }

    public override void ExitState(AIStateController state)
    {
        
    }

    void SearchForLocationCurved()
    {
        float bezierTime = 0;

        Vector2 previousLoc = destPoint;

        float x = 0;
        float y = 0;

        for(int i = 0; i < 100; i++)
        {
            x = Random.Range(-200f, 200f);
            if(x > 40f || x < -40f)
            {
                break;
            }
        }
        for (int i = 0; i < 100; i++)
        {
            y = Random.Range(-150f, 150f);
            if (y > 40f || y < -40f)
            {
                break;
            }
        }

        destPoint = new Vector2(wanderZone.x + x, wanderZone.y + y);

        float curveX = (((1 - bezierTime) * (1 - bezierTime)) * transform.root.position.x) + (2 * bezierTime * (1 - bezierTime) * previousLoc.x) + ((bezierTime * bezierTime) * destPoint.x);
        float curveY = (((1 - bezierTime) * (1 - bezierTime)) * transform.root.position.y) + (2 * bezierTime * (1 - bezierTime) * previousLoc.y) + ((bezierTime * bezierTime) * destPoint.y);

        //finalDestPoint = new Vector2(curveX, curveY);

        walkPointSet = true;
    }

    void IdleMovement()
    {

        if (!walkPointSet)
        {
            SearchForLocationCurved();
        }
        if(walkPointSet)
        {
            transform.root.position = Vector2.MoveTowards(transform.position, destPoint, controller.stats.speed * Time.deltaTime);
        }
        //when destination is reached
        if (Vector2.Distance(this.transform.root.position, destPoint) < 2)
        {
            walkPointSet = false;
        }
    }

    void SwitchToChaseState(AIStateController state)
    {

        switch (AIManager.AIManagerInstance.GetCurrentState())
        {
            case GlobalAIBehaviourState.Timid:
                agroRangeAdjusted = controller.stats.agroRange;
                break;
            case GlobalAIBehaviourState.Curious:
                agroRangeAdjusted = controller.stats.agroRange + 15;
                break;
            case GlobalAIBehaviourState.Angry:
                agroRangeAdjusted = controller.stats.agroRange + 30;
                break;
            case GlobalAIBehaviourState.Aggresive:
                agroRangeAdjusted = controller.stats.agroRange + 45;
                break;  
        }

        if (Vector3.Distance(this.transform.position, controller.player.transform.position) < agroRangeAdjusted && controller.player.GetComponent<PlayerController>().invisible == false)
        {
            state.SwitchToTheNextState(state.ChaseState);
        }
    }
}
