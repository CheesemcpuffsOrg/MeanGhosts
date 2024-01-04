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

    float defaultLightIntensity;

    public bool flashLightSwitch => _flashLightSwitch;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLight;

    [SerializeField]int ghostsWithinRange = 0;

    // Start is called before the first frame update
    void Start()
    {
        normalBeam.enabled = false;
        highBeam.enabled = false;

        defaultLightIntensity = normalBeam.intensity;
    }

    public void FlashLightSwitch()
    {
        AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

        _flashLightSwitch = !_flashLightSwitch;

        if (_flashLightSwitch)
        {
            normalBeam.enabled = true;
            highBeam.enabled = false;
        }
        else if (!_flashLightSwitch)
        {
            normalBeam.enabled = false;
            highBeam.enabled = false;
        }
    }

    public void BeamControl()
    {
        AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

        beamControl = !beamControl;

        if (_flashLightSwitch && beamControl)
        {
            normalBeam.enabled = false;
            highBeam.enabled = true;
        }
        else if(_flashLightSwitch && !beamControl)
        {
            normalBeam.enabled = true;
            highBeam.enabled = false;
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
        } 
        else if (ghostsWithinRange <= 0)
        {
            StopAllCoroutines();
            normalBeam.intensity = defaultLightIntensity;
        }
    }

    IEnumerator Flicker()
    {
        beamControl = false;

        normalBeam.intensity = 0.1f;

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
