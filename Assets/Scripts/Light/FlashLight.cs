using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

//Joshua 2023/12/02

public class FlashLight : MonoBehaviour
{
    public enum FlashLightState { OFF, ON, HIGHBEAM, COOLDOWN, FLICKER }
    [SerializeField] FlashLightState _flashLightState;
    public FlashLightState flashLightState => _flashLightState;

    float globalLightDefaultIntensity = 0.04f;
    [SerializeField] Light2D globalLight;

    [Header ("Default Beam")]
    [SerializeField] Light2D normalBeam;
    [SerializeField]float defaultNormalBeamIntensity = 0.3f;

    [Header ("High Beam")]
    [SerializeField] Light2D highBeam;
    [SerializeField] float defaultHighBeamIntensity = 0.5f;
    [SerializeField] float highBeamDuration = 3;
    Coroutine highBeamPoweringUp;
    public UnityEvent highBeamIsActive { get; } = new UnityEvent();
    [SerializeField] float torchCooldownDuration = 2;

    [Header ("Flicker")]
    [SerializeField]int ghostsWithinRange = 0;
    [SerializeField]int ghostHasBeenCaught = 0;
    List<Coroutine> flickers = new List<Coroutine>();

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
        if (_flashLightState == FlashLightState.HIGHBEAM)
        {
            highBeam.intensity += 0.01f;
        }
    }

    public void FlashLightSwitch()
    {
        if(_flashLightState != FlashLightState.COOLDOWN)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            DefaultLightSwitch();
        }
    }

    private void DefaultLightSwitch()
    {
        if (_flashLightState == FlashLightState.OFF)
        {
            globalLight.intensity = globalLightDefaultIntensity;
            normalBeam.intensity = defaultNormalBeamIntensity;
            highBeam.intensity = 0;
            _flashLightState = FlashLightState.ON;
        }
        else if (_flashLightState == FlashLightState.ON || _flashLightState == FlashLightState.HIGHBEAM || _flashLightState == FlashLightState.FLICKER)
        {
            globalLight.intensity = 0;
            normalBeam.intensity = 0;
            highBeam.intensity = 0;
            _flashLightState = FlashLightState.OFF;

            if (flickers != null)
            {
                foreach (var flicker in flickers)
                {
                    StopCoroutine(flicker);
                }
            }

            if (highBeamPoweringUp != null)
            {
                StopCoroutine(highBeamPoweringUp);
            }
        }
    }

    public void HighBeamControl()
    {
        if (_flashLightState == FlashLightState.ON || _flashLightState == FlashLightState.FLICKER)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            normalBeam.intensity = 0;
            highBeam.intensity = defaultHighBeamIntensity;
            globalLight.intensity = 0;
            highBeamPoweringUp = StartCoroutine(HighBeamPoweringUp());
            highBeamIsActive.Invoke();
            _flashLightState = FlashLightState.HIGHBEAM;

            if (flickers != null)
            {
                foreach (var flicker in flickers)
                {
                    StopCoroutine(flicker);
                }
            }
        }
        else if (_flashLightState == FlashLightState.HIGHBEAM)
        {
            AudioManager.AudioManagerInstance.PlaySound(flashLight, this.gameObject);

            globalLight.intensity = globalLightDefaultIntensity;
            normalBeam.intensity = defaultNormalBeamIntensity;
            highBeam.intensity = 0;
            StopCoroutine(highBeamPoweringUp);
            _flashLightState = FlashLightState.ON; 
        }

    }

    public void IsGhostWithinRange(bool result)
    {
        if (result)
        {
            ghostsWithinRange++;
            FlickeringTorch();
        }
        else
        {
            ghostsWithinRange--;
            FlickeringTorch();
        }
    }

    public void GhostHasBeenCaught(bool result)
    {
        if (result)
        {
            ghostHasBeenCaught++;
            FlickeringTorch();
        }
        else
        {
            ghostHasBeenCaught--;
            FlickeringTorch();
        }
    }

    private void FlickeringTorch()
    {
        if (_flashLightState == FlashLightState.ON || _flashLightState == FlashLightState.FLICKER)
        {
            if (ghostHasBeenCaught != ghostsWithinRange)
            {
                Debug.Log("time to flicker");
                flickers.Add(StartCoroutine(Flicker()));
            }
            else if (ghostHasBeenCaught == ghostsWithinRange)
            {
                Debug.Log("stop flicker");

                _flashLightState = FlashLightState.ON;

                if (flickers != null)
                {
                    foreach (var flicker in flickers)
                    {
                        StopCoroutine(flicker);
                    }

                    normalBeam.intensity = defaultNormalBeamIntensity;
                }
            }
        }  
    }

    IEnumerator Flicker()
    {
        _flashLightState = FlashLightState.FLICKER;

        normalBeam.intensity = 0.1f;
        highBeam.intensity = 0;

        yield return new WaitForSeconds(Random.Range(.1f,.2f));

        normalBeam.intensity = defaultNormalBeamIntensity;

        yield return new WaitForSeconds(Random.Range(.2f, 3f));


        FlickeringTorch(); 
    }

    IEnumerator HighBeamPoweringUp()
    {
        yield return new WaitForSeconds(highBeamDuration);

        _flashLightState = FlashLightState.COOLDOWN;

        normalBeam.intensity = 0;
        highBeam.intensity = 0;

        yield return new WaitForSeconds(torchCooldownDuration);

        _flashLightState = FlashLightState.OFF;
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            normalBeam.intensity = 0;
            highBeam.intensity = 0;

            _flashLightState = FlashLightState.OFF;

            if (highBeamPoweringUp != null)
            {
                StopCoroutine(highBeamPoweringUp);
            }
        }
        else
        {
            DefaultLightSwitch();
        }
    }
}
