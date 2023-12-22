using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//Joshua 2023/12/02

public class FlashLight : MonoBehaviour
{

    [SerializeField] Light2D highBeam;
    [SerializeField] Light2D normalBeam;

    bool beamControl = false;
    [SerializeField] bool flashLightSwitch = false;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLight;

    // Start is called before the first frame update
    void Start()
    {
        normalBeam.enabled = false;
        highBeam.enabled = false;
    }

    public void FlashLightSwitch()
    {
        AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

        flashLightSwitch = !flashLightSwitch;

        if (flashLightSwitch)
        {
            normalBeam.enabled = true;
            highBeam.enabled = false;
        }
        else if (!flashLightSwitch)
        {
            normalBeam.enabled = false;
            highBeam.enabled = false;
        }
    }

    public void BeamControl()
    {
        beamControl = !beamControl;

        //play electric sound when flashlight changes

        if (flashLightSwitch && beamControl)
        {
            normalBeam.enabled = false;
            highBeam.enabled = true;
        }
        else if(flashLightSwitch && !beamControl)
        {
            normalBeam.enabled = true;
            highBeam.enabled = false;
        }
    }
}
