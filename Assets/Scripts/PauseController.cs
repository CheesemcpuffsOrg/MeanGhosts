using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour 
{
    bool started;

    private void Start()
    {
        started = true;
        OnStartOrEnable();
    }

    void Pause(bool isPaused)
    {
        UIContainer.UIContainerInstance.PauseText(isPaused);

        if (isPaused)
        {
            InputManager.InputManagerInstance.DisablePlayerActions();
            AudioManager.AudioManagerInstance.PauseAllAudio();
            Time.timeScale = 0;
        }
        else
        {
            InputManager.InputManagerInstance.EnablePlayerActions();
            AudioManager.AudioManagerInstance.UnPauseAllAudio();
            Time.timeScale = 1;
        }

    }

    void OnStartOrEnable()
    {
        InputManager.InputManagerInstance.pauseEvent += Pause;
    }

    private void OnEnable()
    {
        if(started) { OnStartOrEnable(); }
    }

    private void OnDisable()
    {
        InputManager.InputManagerInstance.pauseEvent -= Pause;
    }
}
