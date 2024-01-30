using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

//Joshua 2023/12/02

public class FlashLight : MonoBehaviour
{

    [SerializeField] Light2D highBeam;
    [SerializeField] Light2D normalBeam;
    [SerializeField] Light2D globalLight;

    bool _beamControl = false;
    bool _flashLightSwitch = false;
    bool torchOnCooldown = false; 

    [SerializeField]float defaultNormalBeamIntensity = 0.3f;
    [SerializeField]float defaultHighBeamIntensity = 0.5f;
    float globalLightDefaultIntensity = 0.04f;

    [SerializeField] float highBeamDuration = 3;
    [SerializeField] float torchCooldownDuration = 2;

    public bool flashLightSwitch => _flashLightSwitch;
    public bool beamControl => _beamControl;

    public UnityEvent highBeamIsActive { get; } = new UnityEvent();

    int ghostsWithinRange = 0;

    List<Coroutine> flickers = new List<Coroutine>();
    Coroutine highBeamPoweringUp;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLight;


    // Start is called before the first frame update
    void Start()
    {
        normalBeam.intensity = 0;
        highBeam.intensity = 0;

    }

    private void Update()
    {
        if (_beamControl)
        {
            highBeam.intensity += 0.01f;
        }
    }

    public void FlashLightSwitch()
    {
        if(!torchOnCooldown)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            _flashLightSwitch = !_flashLightSwitch;

            NormalBeamControl();
        }

    }

    private void NormalBeamControl()
    {
        if (_flashLightSwitch)
        {
            globalLight.intensity = globalLightDefaultIntensity;
            normalBeam.intensity = defaultNormalBeamIntensity;
            highBeam.intensity = 0;
        }
        else if (!_flashLightSwitch)
        {
            globalLight.intensity = 0;
            normalBeam.intensity = 0;
            highBeam.intensity = 0;
            _beamControl = false;

            if (highBeamPoweringUp != null)
            {
                StopCoroutine(highBeamPoweringUp);
            }
        }
    }

    //add cd after use 
    public void BeamControl()
    {
        if(_flashLightSwitch)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            _beamControl = !_beamControl;

            if (_beamControl)
            {
                normalBeam.intensity = 0;
                highBeam.intensity = defaultHighBeamIntensity;
                globalLight.intensity = 0;
                highBeamPoweringUp = StartCoroutine(HighBeamPoweringUp());
                highBeamIsActive.Invoke();
            }
            else if (!_beamControl)
            {
                globalLight.intensity = globalLightDefaultIntensity;
                normalBeam.intensity = defaultNormalBeamIntensity;
                highBeam.intensity = 0;
                StopCoroutine(highBeamPoweringUp);
            }
        } 
    }

    public void FlickeringTorch(bool result)
    {
        if (!torchOnCooldown)
        {
            if (result)
            {
                ghostsWithinRange++;
            }
            else
            {
                ghostsWithinRange--;
            }

            if (ghostsWithinRange > 0 && _flashLightSwitch)
            {
                flickers.Add(StartCoroutine(Flicker()));
            }
            else if (ghostsWithinRange <= 0)
            {
                foreach (var flicker in flickers)
                {
                    StopCoroutine(flicker);
                }

                if (!highBeam)
                {
                    normalBeam.intensity = defaultNormalBeamIntensity;
                }
                
                
            }
        }  
    }

    IEnumerator Flicker()
    {
        _beamControl = false;

        normalBeam.intensity = 0.1f;
        highBeam.intensity = 0;

        yield return new WaitForSeconds(Random.Range(.1f,.2f));

        _beamControl = false;

        normalBeam.intensity = defaultNormalBeamIntensity;

        yield return new WaitForSeconds(Random.Range(.2f, 3f));

        if( ghostsWithinRange > 0)
        {
            StartCoroutine(Flicker());
        } 
    }

    IEnumerator HighBeamPoweringUp()
    {
        yield return new WaitForSeconds(highBeamDuration);

        torchOnCooldown = true;

        normalBeam.intensity = 0;
        highBeam.intensity = 0;
        _flashLightSwitch = false;
        _beamControl = false;

        yield return new WaitForSeconds(torchCooldownDuration);

        torchOnCooldown = false;
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            normalBeam.intensity = 0;
            highBeam.intensity = 0;
            _beamControl = false;

            if (highBeamPoweringUp != null)
            {
                StopCoroutine(highBeamPoweringUp);
            }
        }
        else
        {
            NormalBeamControl();
        }
    }
}
