using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{

    public AIState currentState;
    public AIIdleState IdleState;
    public AIChaseState ChaseState;

    bool delayDone = false;

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    //this delay stops the error when instantiating AI, the problem is with the start method calling late
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);

        currentState = IdleState;

        currentState.EnterState(this);

        delayDone = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (delayDone == true)
        {
            currentState.UpdateState(this);
        }

    }

    public void SwitchToTheNextState(AIState nextState)
    {
        currentState.ExitState(this);
        currentState = nextState;
        currentState.EnterState(this);
    }
}
