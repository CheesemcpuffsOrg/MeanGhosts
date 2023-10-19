using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public ControlScheme controlScheme;

    [SerializeField]Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    [SerializeField] float speed;
    [SerializeField] GameObject flashLight;
    bool flashLightState = false;

    Rigidbody2D rb;

    [SerializeField]bool isMoving = false;
    [SerializeField] bool footsteps = false;

    private void Awake()
    {
        controlScheme = new ControlScheme();
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        rb = GetComponent<Rigidbody2D>();
        flashLight.SetActive(false);
    }

    private void FixedUpdate()
    {
        SmoothMovement();

        PlayFootsteps();
    }

    void SmoothMovement()
    {
        smoothCurrentMoveInput = Vector2.SmoothDamp(smoothCurrentMoveInput, currentMoveInput, ref currentMovementSmoothVelocity, 0.1f);

        rb.velocity = smoothCurrentMoveInput * speed;
        
    }

    void Movement(InputAction.CallbackContext move)
    {
        currentMoveInput = move.ReadValue<Vector2>();   
    }

    void MovementStopped(InputAction.CallbackContext move)
    {
        currentMoveInput = move.ReadValue<Vector2>();   
    }

    void PlayFootsteps()
    {
        if(currentMoveInput.y != 0 || currentMoveInput.x != 0) { isMoving = true; } else { isMoving = false; }

        if(isMoving && !footsteps)
        {
            SoundManager.SoundManagerInstance.PlaySound("Footsteps");
            footsteps = true;
        }
        else if(!isMoving && footsteps)
        {
            SoundManager.SoundManagerInstance.StopSound("Footsteps");
            footsteps = false;
        }
    }

    void FlashLight(InputAction.CallbackContext onoff)
    {
        SoundManager.SoundManagerInstance.PlayOneShotSound("Torch");

        flashLightState = !flashLightState;

        if(flashLightState)
        {
            flashLight.SetActive(true);
        }
        else
        {
            flashLight.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controlScheme.Enable();
    }

    private void OnDisable()
    {
        controlScheme.Disable();
    }
}
