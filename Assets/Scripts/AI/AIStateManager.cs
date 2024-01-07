using System.Collections;
using UnityEngine;

//Joshua

public class AIStateManager : MonoBehaviour
{
    [SerializeField] AIState _CurrentState;
    [SerializeField] AIAnyState _AnyState;
    [SerializeField] AIIdleState _IdleState;
    [SerializeField] AIChaseState _ChaseState;
    [SerializeField] AICaughtState _CaughtState;

    public AIState CurrentState => _CurrentState;
    public AIIdleState IdleState => _IdleState;
    public AIChaseState ChaseState => _ChaseState;
    public AICaughtState CaughtState => _CaughtState;
    public AIAnyState AnyState => _AnyState;

    bool _delayDone = false;
    

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    //this delay stops the error when instantiating AI, the problem is with the start method calling late
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);

        _CurrentState = _IdleState;

        CurrentState.EnterState(this);

        _delayDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_delayDone)
        {
            CurrentState.UpdateState(this);
            _AnyState.UpdateState(this);
        }
    }

    public void SwitchToTheNextState(AIState nextState)
    {
        CurrentState.ExitState(this);
        _CurrentState = nextState;
        CurrentState.EnterState(this);
    }
}
