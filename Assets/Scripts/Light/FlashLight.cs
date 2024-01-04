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

    bool beamControl = false;
    bool _flashLightSwitch = false;
    bool flickering = false;

    [SerializeField]float defaultLightIntensity;
    [SerializeField]float defaultHighBeamIntensity;

    public bool flashLightSwitch => _flashLightSwitch;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLight;

    [SerializeField]int ghostsWithinRange = 0;

    // Start is called before the first frame update
    void Start()
    {
        normalBeam.intensity = 0;
        highBeam.intensity = 0;
    }

    public void FlashLightSwitch()
    {
        AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

        _flashLightSwitch = !_flashLightSwitch;

        if (_flashLightSwitch)
        {
            normalBeam.intensity = defaultLightIntensity;
            highBeam.intensity = 0;
        }
        else if (!_flashLightSwitch)
        {
            normalBeam.intensity = 0;
            highBeam.intensity = 0;
        }
    }

    public void BeamControl()
    {
        if(!flickering)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            beamControl = !beamControl;

            if (_flashLightSwitch && beamControl)
            {
                normalBeam.intensity = 0;
                highBeam.intensity = defaultHighBeamIntensity;
            }
            else if (_flashLightSwitch && !beamControl)
            {
                normalBeam.intensity = defaultLightIntensity;
                highBeam.intensity = 0;
            }
        }
        
    }

    public void FlickeringTorch(bool result)
    {
        if(result)
        {
            ghostsWithinRange++;
        }
        else
        {
            ghostsWithinRange--;
        }

        if(ghostsWithinRange > 0 && _flashLightSwitch)
        {
            StartCoroutine(Flicker());
            flickering = true;
        } 
        else if (ghostsWithinRange <= 0)
        {
            StopAllCoroutines();
            normalBeam.intensity = defaultLightIntensity;
            flickering = false;
        }
    }

    IEnumerator Flicker()
    {
        beamControl = false;

        normalBeam.intensity = 0.1f;
        highBeam.intensity = 0;

        yield return new WaitForSeconds(Random.Range(.1f,.2f));

        beamControl = false;

        normalBeam.intensity = defaultLightIntensity;

        yield return new WaitForSeconds(Random.Range(.2f, 3f));

        if( ghostsWithinRange > 0)
        {
            StartCoroutine(Flicker());
        } 
    }
}
