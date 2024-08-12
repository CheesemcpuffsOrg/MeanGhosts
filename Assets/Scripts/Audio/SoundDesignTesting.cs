using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SoundDesignTesting : MonoBehaviour
{
    public ControlScheme controlScheme;

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    bool shiftIsHeld;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject one;
    [SerializeField] AudioScriptableObject two;
    [SerializeField] AudioScriptableObject three;
    [SerializeField] AudioScriptableObject four;
    [SerializeField] AudioScriptableObject five;
    [SerializeField] AudioScriptableObject six;
    [SerializeField] AudioScriptableObject seven;
    [SerializeField] AudioScriptableObject eight;
    [SerializeField] AudioScriptableObject nine;


    private void Awake()
    {
        controlScheme = new ControlScheme();

        controlScheme.SoundDesignTesting.one.performed += PlaySoundOne;
        controlScheme.SoundDesignTesting.two.performed += PlaySoundTwo;
        controlScheme.SoundDesignTesting.three.performed += PlaySoundThree;
        controlScheme.SoundDesignTesting.four.performed += PlaySoundFour;
        controlScheme.SoundDesignTesting.five.performed += PlaySoundFive;
        controlScheme.SoundDesignTesting.six.performed += PlaySoundSix;
        controlScheme.SoundDesignTesting.seven.performed += PlaySoundSeven;
        controlScheme.SoundDesignTesting.eight.performed += PlaySoundEight;
        controlScheme.SoundDesignTesting.nine.performed += PlaySoundNine;
        controlScheme.SoundDesignTesting.zero.performed += PlaySoundZero;

        controlScheme.SoundDesignTesting.shift.performed += ShiftHeld;
        controlScheme.SoundDesignTesting.shift.canceled += ShiftHeld;

    }

    void PlaySoundOne(InputAction.CallbackContext playOne)
    {
        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        if(!shiftIsHeld)
        {
            soundComponent.PlaySound(one, PleaseFireMe);
        }
        else
        {
            soundComponent.StopSound(one);
        }
        
    }

    void PleaseFireMe()
    {
        Debug.Log("event fired");
    }

    void PlaySoundTwo(InputAction.CallbackContext playTwo)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(two);
        }
        else
        {
            soundComponent.StopSound(two);
        }

    }

    void PlaySoundThree(InputAction.CallbackContext playThree)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(three);
        }
        else
        {
            soundComponent.StopSound(three);
        }

    }

    void PlaySoundFour(InputAction.CallbackContext playFour)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(four);
        }
        else
        {
            soundComponent.StopSound(four);
        }

    }

    void PlaySoundFive(InputAction.CallbackContext playFive)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(five);
        }
        else
        {
            soundComponent.StopSound(five);
        }

    }

    void PlaySoundSix(InputAction.CallbackContext playSix)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(six);
        }
        else
        {
            soundComponent.StopSound(six);
        }

    }

    void PlaySoundSeven(InputAction.CallbackContext playSeven)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(seven);
        }
        else
        {
            soundComponent.StopSound(seven);
        }

    }

    void PlaySoundEight(InputAction.CallbackContext playEight)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(eight);
        }
        else
        {
            soundComponent.StopSound(eight);
        }

    }

    void PlaySoundNine(InputAction.CallbackContext playNine)
    {
        if (!shiftIsHeld)
        {
            soundComponent.PlaySound(nine);
        }
        else
        {
            soundComponent.StopSound(nine);
        }

    }

    void PlaySoundZero(InputAction.CallbackContext zero)
    {
        AudioManager.AudioManagerInstance.StopAllAudio();
    }

    void ShiftHeld(InputAction.CallbackContext held)
    {
        if (held.performed)
        {
            shiftIsHeld = true;
        }
        else if(held.canceled)
        {
            shiftIsHeld = false;
        }
    }

    public void OnEnable()
    {
        controlScheme.SoundDesignTesting.Enable();
    }

    public void OnDisable()
    {
        controlScheme.SoundDesignTesting.Disable();
    }


}
