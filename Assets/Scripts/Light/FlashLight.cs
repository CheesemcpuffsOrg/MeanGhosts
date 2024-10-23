
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

//Joshua 2023/12/02
public enum FlashLightState 
{ 
    OFF, 
    ON, 
    HIGHBEAM, 
    COOLDOWN, 
    FLICKER 
}

public class FlashLight : MonoBehaviour
{

    private FlashLightState _flashLightState;
    FlashLightState flashLightState
    {
        get { return _flashLightState; }

        set
        {
            if (_flashLightState != value)
            {
                _flashLightState = value;
                FlashLightStateChange?.Invoke(_flashLightState);
            }
        }
    }

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    public event Action<FlashLightState> FlashLightStateChange;

    float globalLightDefaultIntensity = 0.04f;
    [SerializeField] Light2D globalLight;

    [Header ("Default Beam")]
    [SerializeField] Light2D normalBeam;
    [SerializeField] float defaultNormalBeamIntensity = 0.3f;
    [SerializeField] List<SpriteMask> torchSpriteMasks;

    [Header ("High Beam")]
    [SerializeField] Light2D highBeam;
    [SerializeField] float defaultHighBeamIntensity = 0.5f;
    [SerializeField] float highBeamDuration = 3;
    Coroutine highBeamPoweringUp;
    public UnityEvent highBeamIsActive { get; } = new UnityEvent();
    [SerializeField] float torchCooldownDuration = 2;

    [Header("Flicker")]
    [SerializeField] int ghostsVisibleToScreen = 0;
    [SerializeField] int ghostsWithinRange = 0;
    [SerializeField] int ghostHasBeenCaught = 0;
    //List<Coroutine> flickers = new List<Coroutine>();
    Coroutine flicker;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject flashLightSwitch;
    [SerializeField] AudioScriptableObject flashLightCharge;
    [SerializeField] AudioScriptableObject flashLightFlicker;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {

        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        normalBeam.intensity = 0;
        highBeam.intensity = 0;

        started = true;
        OnStartOrEnable();

        foreach (var spriteMask in torchSpriteMasks)
        {
            spriteMask.enabled = false;
        }
    }

    private void Update()
    {
        if (flashLightState == FlashLightState.HIGHBEAM && !GameManager.GameManagerInstance.isPaused)
        {
            highBeam.intensity += 0.01f;
        }
    }

    private void DefaultLightSwitch()
    {
        if (flashLightState != FlashLightState.COOLDOWN)
        {
            soundComponent.PlaySound(flashLightSwitch, transform, true);

            if (flashLightState == FlashLightState.OFF)
            {
                globalLight.intensity = globalLightDefaultIntensity;
                normalBeam.intensity = defaultNormalBeamIntensity;
                highBeam.intensity = 0;
                flashLightState = FlashLightState.ON;
                
                FlickeringTorch();

                foreach (var spriteMask in torchSpriteMasks)
                {
                    spriteMask.enabled = true;
                }
            }
            else if (flashLightState == FlashLightState.ON || flashLightState == FlashLightState.HIGHBEAM || flashLightState == FlashLightState.FLICKER)
            {
                globalLight.intensity = 0;
                normalBeam.intensity = 0;
                highBeam.intensity = 0;
                flashLightState = FlashLightState.OFF;

                /*if (flickers != null)
                {
                    foreach (var flicker in flickers)
                    {
                        StopCoroutine(flicker);
                    }
                }*/

                if(flicker != null)
                {
                    StopCoroutine(flicker);
                    flicker = null;
                }

                if (highBeamPoweringUp != null)
                {
                    StopCoroutine(highBeamPoweringUp);
                }

                foreach(var spriteMask in torchSpriteMasks)
                {
                    spriteMask.enabled = false;
                }
            }
        }
    }

    public void HighBeamControl()
    {
        if (flashLightState == FlashLightState.ON || flashLightState == FlashLightState.FLICKER)
        {
            soundComponent.PlaySound(flashLightSwitch, transform, true);
            soundComponent.PlaySound(flashLightCharge, transform, true);

            normalBeam.intensity = 0;
            highBeam.intensity = defaultHighBeamIntensity;
            globalLight.intensity = 0;
            highBeamPoweringUp = StartCoroutine(HighBeamOverCharge());
            highBeamIsActive.Invoke();
            flashLightState = FlashLightState.HIGHBEAM;

            /*if (flickers != null)
            {
                foreach (var flicker in flickers)
                {
                    StopCoroutine(flicker);
                }
            }*/

            if (flicker != null)
            {
                StopCoroutine(flicker);
                flicker = null;
            }
        }
        else if (flashLightState == FlashLightState.HIGHBEAM)
        {
            soundComponent.PlaySound(flashLightSwitch, transform, true);
            soundComponent.StopSound(flashLightCharge);

            globalLight.intensity = globalLightDefaultIntensity;
            normalBeam.intensity = defaultNormalBeamIntensity;
            highBeam.intensity = 0;
            StopCoroutine(highBeamPoweringUp);
            flashLightState = FlashLightState.ON;
            FlickeringTorch();
        }

    }

    /*public void IsGhostOnScreen(bool result)
    {
        if (result)
        {
            ghostsVisibleToScreen++;
            FlickeringTorch();
        }
        else
        {
            ghostsVisibleToScreen--;
            FlickeringTorch();
        }
    }*/

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

    public void HasGhostBeenCaught(bool result)
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
        if (flashLightState == FlashLightState.ON || flashLightState == FlashLightState.FLICKER)
        {
            if (ghostHasBeenCaught == ghostsWithinRange)
            {
                flashLightState = FlashLightState.ON;

                /*if (flickers != null)
                {
                    foreach (var flicker in flickers)
                    {
                        StopCoroutine(flicker);
                    }

                    normalBeam.intensity = defaultNormalBeamIntensity;
                }*/

                if (flicker != null)
                {
                    StopCoroutine(flicker);
                    flicker = null;
                }

                normalBeam.intensity = defaultNormalBeamIntensity;

            }
            else
            {
                // flickers.Add(StartCoroutine(Flicker()));
                if (gameObject.activeSelf)
                {
                    flicker = StartCoroutine(Flicker());
                }   
                
            }
            
        }  
    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(1f);

        flashLightState = FlashLightState.FLICKER;

        normalBeam.intensity = 0.1f;
        highBeam.intensity = 0;

        soundComponent.PlaySound(flashLightFlicker, transform, true);

        yield return new WaitForSeconds(UnityEngine.Random.Range(.1f,.2f));

        normalBeam.intensity = defaultNormalBeamIntensity;

        soundComponent.PlaySound(flashLightFlicker, transform, true);

        yield return new WaitForSeconds(UnityEngine.Random.Range(.2f, 3f));

        FlickeringTorch(); 
    }

    IEnumerator HighBeamOverCharge()
    {
        yield return new WaitForSeconds(highBeamDuration);

        flashLightState = FlashLightState.COOLDOWN;

        normalBeam.intensity = 0;
        highBeam.intensity = 0;

        soundComponent.StopSound(flashLightCharge);

        yield return new WaitForSeconds(torchCooldownDuration);

        flashLightState = FlashLightState.OFF;

    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            normalBeam.intensity = 0;
            highBeam.intensity = 0;
            globalLight.intensity = 0;

            if (highBeamPoweringUp != null)
            {
                StopCoroutine(highBeamPoweringUp);
                flashLightState = FlashLightState.OFF;
                soundComponent.StopSound(flashLightCharge);
            }
        }
        else
        {
            if(flashLightState != FlashLightState.OFF)
            {
                highBeam.intensity = 0;
                globalLight.intensity = globalLightDefaultIntensity;
                normalBeam.intensity = defaultNormalBeamIntensity;
                flashLightState = FlashLightState.ON;
                FlickeringTorch();
            }
        }
    }

    private void OnStartOrEnable()
    {
        InputManager.InputManagerInstance.FlashlightEvent += DefaultLightSwitch;
        InputManager.InputManagerInstance.HighBeamEvent += HighBeamControl;
        InputManager.InputManagerInstance.pauseEvent += Pause;
    }

    private void OnEnable()
    {
        if (started) OnStartOrEnable();
    }

    private void OnDisable()
    {
        InputManager.InputManagerInstance.FlashlightEvent -= DefaultLightSwitch;
        InputManager.InputManagerInstance.HighBeamEvent -= HighBeamControl;
        InputManager.InputManagerInstance.pauseEvent -= Pause;
    }
}
