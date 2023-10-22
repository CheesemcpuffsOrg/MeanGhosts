using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{

    public ControlScheme controlScheme;

    Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    [SerializeField] float speed;
    //float rotationSpeed;
    [SerializeField] GameObject flashLight;
    bool flashLightState = false;

    Rigidbody2D rb;

    bool isMoving = false;
    bool footsteps = false;
    public bool invisible = false;
    [SerializeField]bool safe = false;

    public UnityEvent interactEvent;

    public GameObject heldObject;

    private void Awake()
    {
        controlScheme = new ControlScheme();
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        controlScheme.Player.Interact.performed += Interact;
        rb = GetComponent<Rigidbody2D>();
        flashLight.SetActive(false);
        invisible = true;
    }

    private void FixedUpdate()
    {
        SmoothMovement();

        PlayFootsteps();

       // PlayerRotation();
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

        if (flashLightState)
        {
            flashLight.SetActive(true);
            if(!safe )
            {
                invisible = false;
            }
            
        }
        else 
        {
            flashLight.SetActive(false);
            if(!safe )
            {
                invisible = true;
            }
            
        }
    }

    void Interact(InputAction.CallbackContext interact)
    {

        interactEvent.Invoke();

        if (heldObject != null)
        {
           // heldObject.SetActive(true);
           // heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
        }
        else
        {
            
        }
        
    }

    public void HeldObject(GameObject holdme)
    {
        heldObject = holdme;
    }

    void PlayerInvisible()
    {
        safe = !safe;
        if (safe)
        {
            invisible = true;
        }
        else
        {
            invisible = false;
        }
    }

   /* void PlayerRotation()
    {
        Vector2 movementDirection = new Vector2(smoothCurrentMoveInput.x, smoothCurrentMoveInput.y);

      //  if (currentMoveInput.x == 1 || currentMoveInput.x == -1 || currentMoveInput.y == 1 || currentMoveInput.y == -1)
      //  {

        if(movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
            
       // }
        *//*else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }*//*

    }*/

    private void OnEnable()
    {
        controlScheme.Enable();
    }

    private void OnDisable()
    {
        controlScheme.Disable();
    }
}
