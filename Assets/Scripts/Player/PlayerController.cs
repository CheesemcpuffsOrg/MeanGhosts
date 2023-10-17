using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public ControlScheme controlScheme;

    Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    [SerializeField] float speed;

    Rigidbody2D rb;

    private void Awake()
    {
        controlScheme = new ControlScheme();
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        smoothCurrentMoveInput = Vector2.SmoothDamp(smoothCurrentMoveInput,currentMoveInput, ref currentMovementSmoothVelocity, 0.1f);

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

    private void OnEnable()
    {
        controlScheme.Enable();
    }

    private void OnDisable()
    {
        controlScheme.Disable();
    }
}
