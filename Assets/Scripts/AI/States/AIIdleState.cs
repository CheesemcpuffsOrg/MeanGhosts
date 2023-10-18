using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIIdleState : AIState
{
    [SerializeField] AIStateManager stateManager;
    [SerializeField] AIController controller;

    Vector2 wanderZone = new Vector2(0,0);
    [SerializeField] Vector2 destPoint;
    Vector2 finalDestPoint;

    [SerializeField]bool walkPointSet = false;

    public override void EnterState(AIStateManager state)
    {
        destPoint = this.transform.position;
    }

    public override void UpdateState(AIStateManager state)
    {
        IdleMovement();
    }

    public override void ExitState(AIStateManager state)
    {
        
    }

    void SearchForLocationCurved()
    {
        float bezierTime = 0;

        Vector2 previousLoc = destPoint;

        float x = Random.Range(-100f, 100f);
        float y = Random.Range(-100f, 100f);

        destPoint = new Vector2(wanderZone.y + y, wanderZone.x + x);

        float curveX = (((1 - bezierTime) * (1 - bezierTime)) * transform.root.position.x) + (2 * bezierTime * (1 - bezierTime) * previousLoc.x) + ((bezierTime * bezierTime) * destPoint.x);
        float curveY = (((1 - bezierTime) * (1 - bezierTime)) * transform.root.position.y) + (2 * bezierTime * (1 - bezierTime) * previousLoc.y) + ((bezierTime * bezierTime) * destPoint.y);

        finalDestPoint = new Vector2(curveX, curveY);

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

    

}
