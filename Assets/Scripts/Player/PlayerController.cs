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
    float currentSpeed;
    float defaultSpeed;
    float defaultSpeedModifier = 1f;
    [SerializeField] float speedReductionModifier = 0.75f;

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

    [SerializeField] string walkUpReversed;
    [SerializeField] string walkUpRightReversed;
    [SerializeField] string walkRightReversed;
    [SerializeField] string walkDownRightReversed;
    [SerializeField] string walkDownReversed;
    [SerializeField] string walkDownLeftReversed;
    [SerializeField] string walkLeftReversed;
    [SerializeField] string walkUpLeftReversed;

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
        currentSpeed = playerSO.speed;
        defaultSpeed = playerSO.speed;
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

    #region --WALK ANIMATION --
    void WalkAnimation()
    {
        var torchRotation = flashLight.transform.rotation.eulerAngles;
        var velocity = rb.velocity;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        //walk up
        if(velocity.y > 0.5f && velocity.x > -0.5 && velocity.x < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, defaultSpeedModifier);
            }
        }
        // walk up left
        else if (velocity.y > 0.5f && velocity.x < -0.5)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, defaultSpeedModifier) ;
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownReversed, speedReductionModifier); ;
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, speedReductionModifier);
            }
        }
        //walk left
        else if (velocity.x < -0.5f && velocity.y > -0.5 && velocity.y < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDown, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRightReversed, speedReductionModifier);
            }
        }
        //walk down left
        else if (velocity.x < -0.5f && velocity.y < -0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRightReversed, speedReductionModifier);
            }
        }
        //walk down
        else if (velocity.y < -0.5f && velocity.x > -0.5f && velocity.x < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRightReversed, speedReductionModifier);
            }
        }
        //walk down right
        else if (velocity.y < -0.5f && velocity.x > 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, speedReductionModifier);
            }
        }
        //walk right
        else if (velocity.x > 0.5f && velocity.y < 0.5f && velocity.y > -0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDown, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, defaultSpeedModifier);
            }
        }
        //walk right up
        else if (velocity.x > 0.5f && velocity.y > 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, defaultSpeedModifier);
            }
        }
    }

    private void WalkAnimationModifiers(SpriteRenderer spriteRenderer, bool flipSprite, string animationName, float speedMultiplier)
    {
        spriteRenderer.flipX = flipSprite;
        animController.PlayAnimation(animationName);
        currentSpeed = defaultSpeed * speedMultiplier;
    }

    #endregion

    void SmoothMovement()
    {
        smoothCurrentMoveInput = Vector2.SmoothDamp(smoothCurrentMoveInput, currentMoveInput, ref currentMovementSmoothVelocity, 0.1f);

        rb.velocity = smoothCurrentMoveInput * currentSpeed;
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


