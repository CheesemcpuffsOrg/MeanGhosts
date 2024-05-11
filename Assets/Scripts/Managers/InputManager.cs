using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager GameplayInputManagerInstance;

    ControlScheme controlScheme;

    public event Action<Vector2> onMoveInputChangeEvent;
    public event Action interactEvent;
    public event Action FlashlightEvent;
    public event Action HighBeamEvent;

    public event Action<bool> pauseEvent;
    bool isPaused = false;

    void Awake()
    {
        GameplayInputManagerInstance = this;

        controlScheme = new ControlScheme();

        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        controlScheme.Player.Interact.performed += Interact;
        controlScheme.Player.BeamControl.performed += BeamControl;

        controlScheme.Pause.Pause.performed += Pause;
    }

    #region -- PLAYER CONTROLS --
    void Movement(InputAction.CallbackContext move)
    {
        onMoveInputChangeEvent?.Invoke(move.ReadValue<Vector2>());
    }

    void MovementStopped(InputAction.CallbackContext move)
    {
        onMoveInputChangeEvent?.Invoke(move.ReadValue<Vector2>());
    }

    void FlashLight(InputAction.CallbackContext onoff)
    {
        FlashlightEvent?.Invoke();

        UIContainer.UIContainerInstance.DisableLampText();
    }

    void BeamControl(InputAction.CallbackContext beam)
    {
        HighBeamEvent?.Invoke();
    }

    void Interact(InputAction.CallbackContext interact)
    {
        interactEvent?.Invoke();
    }
    #endregion

    #region --PAUSE--
    void Pause(InputAction.CallbackContext pauseGame)
    {
        pauseEvent?.Invoke(isPaused);
        isPaused = !isPaused;
    }
    #endregion

    #region --DISABLE AND ENABLE CONTROLS --
    public void EnablePlayerActions()
    {
        controlScheme.Player.Enable();
    }

    public void DisablePlayerActions()
    {
        controlScheme.Player.Disable();
    }

    public void EnablePauseActions()
    {
        controlScheme.Pause.Enable();
    }

    public void DisablePauseActions()
    {
        controlScheme.Pause.Disable();
    }

    public void EnableAllInputs()
    {
        controlScheme.Enable();
    }

    public void DisableAllInputs()
    {
        controlScheme.Disable();
    }

    private void OnEnable()
    {
        EnableAllInputs();
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }
    #endregion
}
