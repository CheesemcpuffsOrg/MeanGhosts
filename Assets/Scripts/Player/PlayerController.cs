using AudioSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Joshua 2023/12/02


public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerSO playerSO;

    public ControlScheme controlScheme;

    Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    [SerializeField] float speed;

    [SerializeField] FlashLight flashlight;
    public GameObject flashLight;

    Rigidbody2D rb;

    bool isMoving = false;
    bool footsteps = false;
    public bool invisible = false;
    public bool safe = false;

    private UnityEvent _interactEvent;
    public UnityEvent interactEvent => _interactEvent;

    public GameObject heldObject;

    public bool canInteract;

    [Header ("Animation")]
    [SerializeField]AnimationController animController;
    [SerializeField] string idle;
    [SerializeField] string walkUp;
    [SerializeField] string walkUpRight;
    [SerializeField] string walkRight;
    [SerializeField] string walkDownRight;
    [SerializeField] string walkDown;
    [SerializeField] string walkDownLeft;
    [SerializeField] string walkLeft;
    [SerializeField] string walkUpLeft;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject footSteps;

    private void Awake()
    {
        controlScheme = new ControlScheme();
        controlScheme.Player.Move.performed += Movement;
        controlScheme.Player.Move.canceled += MovementStopped;
        controlScheme.Player.Flashlight.performed += FlashLight;
        controlScheme.Player.Interact.performed += Interact;
        controlScheme.Player.BeamControl.performed += BeamControl;
        rb = GetComponent<Rigidbody2D>();
        //invisible = true;
        speed = playerSO.speed;
    }

    private void FixedUpdate()
    {
        if (currentMoveInput.y != 0 || currentMoveInput.x != 0) { isMoving = true; } else { isMoving = false; }

        if (isMoving)
        {
            WalkAnimation();
        }
        else
        {
            animController.PlayAnimation(idle);
        }

        SmoothMovement();

        PlayFootsteps();

        // PlayerRotation();
    }

    void Movement(InputAction.CallbackContext move)
    {
        currentMoveInput = move.ReadValue<Vector2>();
    }

    void MovementStopped(InputAction.CallbackContext move)
    {
        currentMoveInput = move.ReadValue<Vector2>();
    }

    void WalkAnimation()
    {
        if (flashLight.transform.rotation.eulerAngles.z > 337.6f || flashLight.transform.rotation.eulerAngles.z < 22.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animController.PlayAnimation(walkUp);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 22.6f && flashLight.transform.rotation.eulerAngles.z < 67.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animController.PlayAnimation(walkUpLeft);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 67.6f && flashLight.transform.rotation.eulerAngles.z < 112.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animController.PlayAnimation(walkLeft);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 112.6f && flashLight.transform.rotation.eulerAngles.z < 157.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animController.PlayAnimation(walkDownLeft);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 157.6f && flashLight.transform.rotation.eulerAngles.z < 202.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animController.PlayAnimation(walkDown);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 202.6f && flashLight.transform.rotation.eulerAngles.z < 247.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animController.PlayAnimation(walkDownRight);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 247.6f && flashLight.transform.rotation.eulerAngles.z < 292.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animController.PlayAnimation(walkRight);
        }
        else if (flashLight.transform.rotation.eulerAngles.z > 292.6f && flashLight.transform.rotation.eulerAngles.z < 337.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animController.PlayAnimation(walkUpRight);
        }
    }

    void SmoothMovement()
    {

        smoothCurrentMoveInput = Vector2.SmoothDamp(smoothCurrentMoveInput, currentMoveInput, ref currentMovementSmoothVelocity, 0.1f);

        rb.velocity = smoothCurrentMoveInput * speed;

        

    }

    
    void PlayFootsteps()
    {
        
        if (isMoving && !footsteps)
        {
            AudioManager.AudioManagerInstance.PlaySound(footSteps, this.gameObject);
            footsteps = true;
        }
        else if (!isMoving && footsteps)
        {
            AudioManager.AudioManagerInstance.StopSound(footSteps, this.gameObject);
            footsteps = false;
        }
    }

    void FlashLight(InputAction.CallbackContext onoff)
    {
        flashlight.FlashLightSwitch();

        UIManagers.UIManagersInstance.DisableLampText();
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
            interactEvent.AddListener(SafeZoneDrop);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SafeZone")
        {
            PlayerInvisible();
            interactEvent.RemoveListener(SafeZoneDrop);
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
            UIManagers.UIManagersInstance.DisableItemImage();
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


