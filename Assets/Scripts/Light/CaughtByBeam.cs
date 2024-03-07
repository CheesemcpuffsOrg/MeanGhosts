using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaughtByBeam : MonoBehaviour
{
    [System.Serializable]
    public class CaughtEvent : UnityEvent<bool>{}

    GameObject player;

    [Header ("Torch")]
    [SerializeField] List<GameObject> spottedInterfaces = new List<GameObject>();
    FlashLight flashLight;
    CaughtEvent _caughtEvent;
    public CaughtEvent caughtEvent => _caughtEvent;
    CaughtEvent _caughtByHighBeamEvent;
    public CaughtEvent caughtByHighBeamEvent => _caughtByHighBeamEvent;
    bool spotted;
    bool spottedByHighBeam;
    bool caught;
    bool caughtByHighBeam;

    [Header ("Camera")]
    Vector3 viewPos;
    Camera _cam;
    [SerializeField] bool visibleToCamera = false;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerTag;

    private void Awake()
    {
        _caughtEvent = new CaughtEvent();
        _caughtByHighBeamEvent = new CaughtEvent();

        _cam = Camera.main;
    }

    private void Start()
    {
        player = TagExtensions.FindWithTag(gameObject, playerTag);

        flashLight = player.GetComponentInChildren<FlashLight>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCaught();
        
        VisibleToCamera();
    }

    private void CheckIfCaught()
    {
        if (flashLight.flashLightState == FlashLight.FlashLightState.ON)
        {
            if (spotted && visibleToCamera && !caught)
            {
                Debug.Log("spotted");
                caught = true;
                _caughtEvent.Invoke(true);
            }
            else if ((!spotted || !visibleToCamera) && caught)
            {
                Debug.Log("not spotted");
                caught = false;
                _caughtEvent.Invoke(false);
            }
        }
        else if (flashLight.flashLightState == FlashLight.FlashLightState.HIGHBEAM)
        {
            if (spottedByHighBeam && visibleToCamera && !caughtByHighBeam)
            {
                caughtByHighBeam = true;
                _caughtByHighBeamEvent.Invoke(true);
                Debug.Log("caught by high beam");
            }
            else if (!spottedByHighBeam && caughtByHighBeam)
            {
                caughtByHighBeam = false;
                _caughtByHighBeamEvent.Invoke(false);
                //Debug.Log("not caught by highbeam");
            }
        }
        else if(flashLight.flashLightState == FlashLight.FlashLightState.OFF || flashLight.flashLightState == FlashLight.FlashLightState.COOLDOWN)
        {
            _caughtEvent.Invoke(false);
            _caughtByHighBeamEvent.Invoke(false);
        }
    }

    private void VisibleToCamera()
    {
        viewPos = _cam.WorldToViewportPoint(this.transform.position);

        if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
        {
            visibleToCamera = true;
            //Debug.Log("seen by camera");
        }
        else
        {
            visibleToCamera = false;
        }
    }

    public void SpottedByTorch(bool isSpotted)
    {
        foreach(var i in spottedInterfaces)
        {
            ISpotted spotted = i.GetComponent<ISpotted>();

            spotted.SpottedByTorchInterface(isSpotted);
        }

        spotted = isSpotted;

        //Debug.Log("caught by torch");
    }

    public void SpottedByHighBeam(bool isSpotted)
    {
        foreach (var i in spottedInterfaces)
        {
            ISpotted spotted = i.GetComponent<ISpotted>();

            spotted.SpottedByHighBeamInterface(isSpotted);
        }

        spottedByHighBeam = isSpotted;
    }

}
