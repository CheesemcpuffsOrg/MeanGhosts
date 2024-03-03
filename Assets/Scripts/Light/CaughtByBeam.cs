using UnityEngine;
using UnityEngine.Events;

public class CaughtByBeam : MonoBehaviour
{
    [System.Serializable]
    public class CaughtEvent : UnityEvent<bool>{}

    GameObject player;

    [Header ("Torch")]
    bool spottedByTorch = false;
    bool spottedByHighBeam = false;
    FlashLight flashLight;
    bool _caught = false;
    CaughtEvent _caughtEvent;
    public CaughtEvent caughtEvent => _caughtEvent;
    CaughtEvent _caughtByHighBeam;
    public CaughtEvent caughtByHighBeam => _caughtByHighBeam;

    [Header ("Camera")]
    Vector3 viewPos;
    Camera _cam;
    bool visibleToCamera = false;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerTag;

    private void Awake()
    {
        _caughtEvent = new CaughtEvent();
        _caughtByHighBeam = new CaughtEvent();

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
        if (flashLight.flashLightSwitch && !flashLight.beamControl)
        {
            if (!_caught)
            {
                if (visibleToCamera && spottedByTorch)
                {
                    _caught = true;
                    _caughtEvent.Invoke(_caught);
                }
            }

            if (!spottedByTorch)
            {
                _caught = false;
                _caughtEvent.Invoke(_caught);
            }
        }
        else if (flashLight.flashLightSwitch && flashLight.beamControl)
        {
            if (!_caught)
            {
                if (visibleToCamera && spottedByHighBeam)
                {
                    _caught = true;
                    _caughtByHighBeam.Invoke(_caught);
                }
            }

            if (!spottedByHighBeam)
            {
                _caught = false;
                _caughtByHighBeam.Invoke(_caught);
            }
        }
    }

    private void VisibleToCamera()
    {
        viewPos = _cam.WorldToViewportPoint(this.transform.position);

        if (viewPos.x < 1.05f && viewPos.x > -0.05f && viewPos.y < 1.05 && viewPos.y > -0.05f)
        {
            visibleToCamera = true;
        }
        else
        {
            visibleToCamera = false;
        }
    }

    public void SpottedByTorch(bool isSpotted, FlashLight data)
    {
        ISpotted spotted = this.GetComponent<ISpotted>();

        spotted.SpottedByTorchInterface(isSpotted);

        spottedByTorch = isSpotted;

        if (flashLight == null)
        {
            flashLight = data;
        }
    }

    public void SpottedByHighBeam(bool isSpotted, FlashLight data)
    {
        spottedByHighBeam = isSpotted;

        if(flashLight == null)
        {
            flashLight = data;
        }
    }

}
