using System.Collections;
using UnityEngine;

//Joshua

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

    GameObject player;

    FlashLight flashLight;

    Camera cam;

    Vector3 viewPos;

    bool _delayDone = false;
    bool _seenByTorch = false;
    bool _visibleToCamera = false;
    bool _caught = false;

    [SerializeField] int affectLightRange;
    [SerializeField]bool isWithinRange = false;

    [Header("Tags")]
    [SerializeField]TagScriptableObject playerTag;

    public bool caught => _caught;

    private void Start()
    {
        player = TagExtensions.FindWithTag(gameObject, playerTag);
        flashLight = player.GetComponentInChildren<FlashLight>();

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
        if (!isWithinRange )
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < affectLightRange && !caught)
            {
                isWithinRange = true;
                flashLight.FlickeringTorch(isWithinRange);
            }
        }
        else if(isWithinRange)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) > affectLightRange || caught)
            {
                isWithinRange = false;
                flashLight.FlickeringTorch(isWithinRange);
            }
        }

        viewPos = cam.WorldToViewportPoint(this.transform.position);

        if (flashLight.flashLightSwitch)
        {
            if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
            {
                _visibleToCamera = true;
            }
            else
            {
                _visibleToCamera = false;
            }

            if (!caught)
            {
                if (_visibleToCamera && _seenByTorch)
                {
                    _caught = true;
                    SwitchToTheNextState(CaughtState);
                }
            }

            if (!_visibleToCamera || !_seenByTorch)
            {
                _caught = false;
            }
        }
        else
        {
            _caught = false;
        } 
    }

    public void SpottedByTorch(bool isSpotted)
    {
        _seenByTorch = isSpotted;
    }
    #endregion
}
