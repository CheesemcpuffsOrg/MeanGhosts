using AudioSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Joshua 2023/12/02


public class PlayerController : MonoBehaviour
{
    ControlScheme controlScheme;

    public event Action<Vector2> onMoveInputChange;
    public event Action interactEvent;

    [SerializeField] FlashLight flashlight;

    public bool invisible = false;
    public bool safe = false;

    public GameObject heldObject;

    public bool canInteract;

    private void Awake()
    {
        controlScheme = new ControlScheme(); 
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        controlScheme.Player.Interact.performed += Interact;
        controlScheme.Player.BeamControl.performed += BeamControl;
    }

    void Movement(InputAction.CallbackContext move)
    {
        onMoveInputChange.Invoke(move.ReadValue<Vector2>());
    }

    void MovementStopped(InputAction.CallbackContext move)
    {
        onMoveInputChange.Invoke(move.ReadValue<Vector2>());
    }

    void FlashLight(InputAction.CallbackContext onoff)
    {
        flashlight.FlashLightSwitch();

        UIContainer.UIContainerInstance.DisableLampText();
    }

    void BeamControl(InputAction.CallbackContext beam)
    {
        flashlight.HighBeamControl();
    }

    void Interact(InputAction.CallbackContext interact)
    {
        interactEvent.Invoke();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SafeZone")
        {
            PlayerInvisible();
            interactEvent += SafeZoneDrop;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SafeZone")
        {
            PlayerInvisible();
            interactEvent -= SafeZoneDrop;
        }
    }

    void SafeZoneDrop()
    {
        if (heldObject != null && !canInteract)
        {
            heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            SpriteRenderer[] renderers = heldObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            heldObject.GetComponent<Collider2D>().enabled = true;
            heldObject = null;
            //  SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");
            UIContainer.UIContainerInstance.DisableItemImage();
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

    public void OnEnable()
    {
        controlScheme.Player.Enable();
    }

    public void OnDisable()
    {
        controlScheme.Player.Disable();
    }
}


