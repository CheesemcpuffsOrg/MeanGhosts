using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour 
{

    private void Start()
    {
        
    }

    /*void Pause(InputAction.CallbackContext pauseGame)
    {
        _isPaused = !_isPaused;

        UIContainer.UIContainerInstance.PauseText();

        if (_isPaused)
        {
            player.GetComponentInChildren<FlashLight>().Pause(_isPaused);
            InputManager.GameplayInputManagerInstance.DisablePlayerActions();
            AudioManager.AudioManagerInstance.PauseAllAudio();
            Time.timeScale = 0;
        }
        else
        {

            player.GetComponentInChildren<FlashLight>().Pause(_isPaused);
            InputManager.GameplayInputManagerInstance.EnablePlayerActions();
            AudioManager.AudioManagerInstance.UnPauseAllAudio();
            Time.timeScale = 1;
        }

    }*/
}
