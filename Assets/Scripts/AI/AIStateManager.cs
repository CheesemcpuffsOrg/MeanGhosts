using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{

    public AIState _CurrentState;
    public AIIdleState _IdleState;
    public AIChaseState _ChaseState;
    public AICaughtState _CaughtState;

    public AIState CurrentState => _CurrentState;
    public AIIdleState IdleState => _IdleState;
    public AIChaseState ChaseState => _ChaseState;
    public AICaughtState CaughtState => _CaughtState;

    Camera cam;

    [SerializeField] private Vector3 viewPos;

    bool _delayDone = false;
    [SerializeField] bool seenByTorch = false;
    [SerializeField] bool _caught = false;
    [SerializeField] bool visibleToCamera = false;

    public bool caught => _caught;

    private void Start()
    {
        cam = Camera.main;

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
        }

        CheckIfCaught();
    }

    public void SwitchToTheNextState(AIState nextState)
    {
        CurrentState.ExitState(this);
        _CurrentState = nextState;
        CurrentState.EnterState(this);
    }

    #region --ANY STATE--

    private void CheckIfCaught()
    {
        viewPos = cam.WorldToViewportPoint(this.transform.position);

        if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
        {
            visibleToCamera = true;
        }
        else
        {
            visibleToCamera = false;
        }

        if (!caught)
        {
            if (visibleToCamera && seenByTorch)
            {
                _caught = true;
                SwitchToTheNextState(CaughtState);
            }
        }

        if (!visibleToCamera || !seenByTorch)
        {
            _caught = false;
        }
    }

    public void SpottedByTorch(bool isSpotted)
    {
        seenByTorch = isSpotted;
    }
    #endregion
}
