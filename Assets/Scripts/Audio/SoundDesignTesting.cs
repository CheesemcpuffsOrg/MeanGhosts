using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SoundDesignTesting : MonoBehaviour
{
    public ControlScheme controlScheme;

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
        if(!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(one, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(one, this.gameObject);
        }
        
    }

    void PlaySoundTwo(InputAction.CallbackContext playTwo)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(two, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(two, this.gameObject);
        }

    }

    void PlaySoundThree(InputAction.CallbackContext playThree)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(three, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(three, this.gameObject);
        }

    }

    void PlaySoundFour(InputAction.CallbackContext playFour)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(four, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(four, this.gameObject);
        }

    }

    void PlaySoundFive(InputAction.CallbackContext playFive)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(five, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(five, this.gameObject);
        }

    }

    void PlaySoundSix(InputAction.CallbackContext playSix)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(six, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(six, this.gameObject);
        }

    }

    void PlaySoundSeven(InputAction.CallbackContext playSeven)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(seven, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(seven, this.gameObject);
        }

    }

    void PlaySoundEight(InputAction.CallbackContext playEight)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(eight, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(eight, this.gameObject);
        }

    }

    void PlaySoundNine(InputAction.CallbackContext playNine)
    {
        if (!shiftIsHeld)
        {
            AudioManager.AudioManagerInstance.PlaySound(nine, this.gameObject);
        }
        else
        {
            AudioManager.AudioManagerInstance.StopSound(nine, this.gameObject);
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
        controlScheme.Player.Enable();
    }

    public void OnDisable()
    {
        controlScheme.Player.Disable();
    }


}
