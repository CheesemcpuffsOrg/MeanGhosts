using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameplayInputManager : MonoBehaviour
{
    public static GameplayInputManager GameplayInputManagerInstance;

    ControlScheme controlScheme;

    public event Action<Vector2> onMoveInputChangeEvent;
    public event Action interactEvent;
    public event Action FlashlightEvent;
    public event Action HighBeamEvent;

    void Awake()
    {
        GameplayInputManagerInstance = this;

        controlScheme = new ControlScheme();
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        controlScheme.Player.Interact.performed += Interact;
        controlScheme.Player.BeamControl.performed += BeamControl;
    }

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

    public void OnEnable()
    {
        controlScheme.Player.Enable();
    }

    public void OnDisable()
    {
        controlScheme.Player.Disable();
    }
}
