using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//Joshua 2023/12/02

public class FlashLight : MonoBehaviour
{

    [SerializeField] Light2D highBeam;
    [SerializeField] Light2D normalBeam;

    bool _beamControl = false;
    bool _flashLightSwitch = false;
    bool flickering = false;
    bool torchOnCooldown = false; 

    [SerializeField]float defaultNormalBeamIntensity = 0.3f;
    [SerializeField]float defaultHighBeamIntensity = 0.5f;

    [SerializeField] float highBeamDuration = 3;
    [SerializeField] float torchCooldownDuration = 2;

    public bool flashLightSwitch => _flashLightSwitch;
    public bool beamControl => _beamControl;

    int ghostsWithinRange = 0;

    List<Coroutine> flickers = new List<Coroutine>();

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLight;


    // Start is called before the first frame update
    void Start()
    {
        normalBeam.intensity = 0;
        highBeam.intensity = 0;
    }

    public void FlashLightSwitch()
    {
        if(!torchOnCooldown)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            _flashLightSwitch = !_flashLightSwitch;

            if (_flashLightSwitch)
            {
                normalBeam.intensity = defaultNormalBeamIntensity;
                highBeam.intensity = 0;
            }
            else if (!_flashLightSwitch)
            {
                normalBeam.intensity = 0;
                highBeam.intensity = 0;
                _beamControl = false;
            }
        }
        
    }

    //add cd after use 
    public void BeamControl()
    {
        if(!flickering && _flashLightSwitch)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            _beamControl = !_beamControl;

            if (_beamControl)
            {
                normalBeam.intensity = 0;
                highBeam.intensity = defaultHighBeamIntensity;
                StartCoroutine(HighBeamPoweringUp());
            }
            else if (!_beamControl)
            {
                normalBeam.intensity = defaultNormalBeamIntensity;
                highBeam.intensity = 0;
                StopCoroutine(HighBeamPoweringUp());
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
                flickering = true;
            }
            else if (ghostsWithinRange <= 0)
            {
                foreach (var flicker in flickers)
                {
                    StopCoroutine(flicker);
                }

                normalBeam.intensity = defaultNormalBeamIntensity;
                flickering = false;
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
}
