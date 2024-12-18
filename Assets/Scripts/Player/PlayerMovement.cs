
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    [Header("Objects")]
    [SerializeField] AnimationController animController;
    [SerializeField] PlayerScriptableObject playerSO;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] GameObject flashLightObj;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Modifiers")]
    [SerializeField] float speedReductionModifier = 0.5f;

    [Header("Animation")]
    //idle
    [SerializeField] string idleUp;
    [SerializeField] string idleUpRight;
    [SerializeField] string idleRight;
    [SerializeField] string idleDownRight;
    [SerializeField] string idleDown;
    [SerializeField] string idleDownLeft;
    [SerializeField] string idleLeft;
    [SerializeField] string idleUpLeft;
    //walk forward
    [SerializeField] string walkUp;
    [SerializeField] string walkUpRight;
    [SerializeField] string walkRight;
    [SerializeField] string walkDownRight;
    [SerializeField] string walkDown;
    [SerializeField] string walkDownLeft;
    [SerializeField] string walkLeft;
    [SerializeField] string walkUpLeft;
    //walk backwards
    [SerializeField] string walkUpReversed;
    [SerializeField] string walkUpRightReversed;
    [SerializeField] string walkRightReversed;
    [SerializeField] string walkDownRightReversed;
    [SerializeField] string walkDownReversed;
    [SerializeField] string walkDownLeftReversed;
    [SerializeField] string walkLeftReversed;
    [SerializeField] string walkUpLeftReversed;
    //altered walk
    [SerializeField] string UpAlteredwalkLeft;
    [SerializeField] string UpAlteredwalkRight;
    [SerializeField] string UpAlteredwalkUpLeft;
    [SerializeField] string UpAlteredwalkUpRight;
    [SerializeField] string UpAlteredwalkUpLeftReversed;
    [SerializeField] string UpAlteredwalkUpRightReversed;

    [SerializeField] string DownAlteredwalkLeft;
    [SerializeField] string DownAlteredwalkRight;
    [SerializeField] string DownAlteredwalkDownLeft;
    [SerializeField] string DownAlteredwalkDownRight;
    [SerializeField] string DownAlteredwalkDownLeftReversed;
    [SerializeField] string DownAlteredwalkDownRightReversed;

    [SerializeField] string LeftAlteredwalkDown;
    [SerializeField] string LeftAlteredwalkUp;
    [SerializeField] string LeftAlteredwalkLeftDown;
    [SerializeField] string LeftAlteredwalkLeftUp;
    [SerializeField] string LeftAlteredwalkLeftDownReversed;
    [SerializeField] string LeftAlteredwalkLeftUpReversed;

    [SerializeField] string RightAlteredwalkDown;
    [SerializeField] string RightAlteredwalkUp;
    [SerializeField] string RightAlteredwalkRightDown;
    [SerializeField] string RightAlteredwalkRightUp;
    [SerializeField] string RightAlteredwalkRightDownReversed;
    [SerializeField] string RightAlteredwalkRightUpReversed;

    [SerializeField] string UpRightAlteredwalkUp;
    [SerializeField] string UpRightAlteredwalkUpReversed;
    [SerializeField] string UpRightAlteredwalkRight;
    [SerializeField] string UpRightAlteredwalkRightReversed;

    [SerializeField] string UpLeftAlteredwalkUp;
    [SerializeField] string UpLeftAlteredwalkUpReversed;
    [SerializeField] string UpLeftAlteredwalkLeft;
    [SerializeField] string UpLeftAlteredwalkLeftReversed;

    [SerializeField] string DownRightAlteredwalkDown;
    [SerializeField] string DownRightAlteredwalkDownReversed;
    [SerializeField] string DownRightAlteredwalkRight;
    [SerializeField] string DownRightAlteredwalkRightReversed;

    [SerializeField] string DownLeftAlteredwalkDown;
    [SerializeField] string DownLeftAlteredwalkDownReversed;
    [SerializeField] string DownLeftAlteredwalkLeft;
    [SerializeField] string DownLeftAlteredwalkLeftReversed;


    [Header("Sounds")]
    [SerializeField] AudioScriptableObject footSteps;

    Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    float currentSpeed;
    float defaultSpeed;
    float defaultSpeedModifier = 1f;
    bool isMoving = false;
    bool footsteps = false;
    bool started = false;

    // Start is called before the first frame update
    void Start()
    {

        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        currentSpeed = playerSO.speed;
        defaultSpeed = playerSO.speed;

        started = true;
        OnStartOrEnable();
    }

    // Update is called once per frame
    void Update()
    {
        PlayFootsteps();

        var torchRotation = flashLightObj.transform.rotation.eulerAngles;
        
        if (isMoving)
        {
            var velocity = rigidBody.velocity;
            WalkAnimation(velocity, torchRotation);
        }
        else
        {
            IdleAnimation(torchRotation);
        }
    }

    private void FixedUpdate()
    {
        var velocity = rigidBody.velocity;
        if (velocity.y > 0.1 || velocity.x > 0.1 || velocity.y < -0.1 || velocity.x < -0.1) { isMoving = true; } else { isMoving = false; }

        SmoothMovement();
    }

    private void ReactToInput(Vector2 input)
    {
        currentMoveInput = input;
    }

    void SmoothMovement()
    {
        smoothCurrentMoveInput = Vector2.SmoothDamp(smoothCurrentMoveInput, currentMoveInput, ref currentMovementSmoothVelocity, 0.1f);

        rigidBody.velocity = smoothCurrentMoveInput * currentSpeed;
    }


    void PlayFootsteps()
    {
      
        if (isMoving && !footsteps)
        {
            soundComponent.PlaySound(footSteps, transform, true);
            footsteps = true;
        }
        else if (!isMoving && footsteps)
        {
            soundComponent.StopSound(footSteps);
            footsteps = false;
        }
    }

    private void WalkAnimationModifiers(bool flipSprite, string animationName, float speedMultiplier)
    {
        spriteRenderer.flipX = flipSprite;
        animController.PlayAnimation(animationName);
        currentSpeed = defaultSpeed * speedMultiplier;
    }

    private void IdleAnimationModifiers(bool flipSprite, string animationName)
    {
        spriteRenderer.flipX = flipSprite;
        animController.PlayAnimation(animationName);
    }

    #region --IDLE ANIMATION --

    void IdleAnimation(Vector3 torchRotation)
    {
        if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
        {
            IdleAnimationModifiers(false, idleUp);
        }
        else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
        {
            IdleAnimationModifiers(true, idleUpLeft);
        }
        else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
        {
            IdleAnimationModifiers(true, idleLeft);
        }
        else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
        {
            IdleAnimationModifiers(true, idleDownLeft);
        }
        else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
        {
            IdleAnimationModifiers(false, idleDown);
        }
        else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
        {
            IdleAnimationModifiers(false, idleDownRight);
        }
        else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
        {
            IdleAnimationModifiers(false, idleRight);
        }
        else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
        {
            IdleAnimationModifiers(false, idleUpRight);
        }
    }

    #endregion

    #region --WALK ANIMATION --
    void WalkAnimation(Vector2 velocity, Vector3 torchRotation)
    {
        
        //walk up
        if (velocity.y > 0.5f && velocity.x > -0.5 && velocity.x < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, walkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, UpLeftAlteredwalkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, DownLeftAlteredwalkDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, walkDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(true, DownRightAlteredwalkDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, UpRightAlteredwalkUp, defaultSpeedModifier);
            }
        }
        // walk up left
        else if (velocity.y > 0.5f && velocity.x < -0.5)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, UpAlteredwalkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkLeftUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, DownAlteredwalkDownLeftReversed, speedReductionModifier); ;
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(false, walkDownRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkRightDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, walkUpRight, defaultSpeedModifier);
            }
        }
        //walk left
        else if (velocity.x < -0.5f && velocity.y > -0.5 && velocity.y < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, UpAlteredwalkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, UpLeftAlteredwalkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, walkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, DownLeftAlteredwalkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(true, DownAlteredwalkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(true, DownRightAlteredwalkRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, walkRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, UpRightAlteredwalkRightReversed, speedReductionModifier);
            }
        }
        //walk down left
        else if (velocity.x < -0.5f && velocity.y < -0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, UpAlteredwalkUpLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkLeftDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, DownAlteredwalkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkRightUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, walkUpRightReversed, speedReductionModifier);
            }
        }
        //walk down
        else if (velocity.y < -0.5f && velocity.x > -0.5f && velocity.x < 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, walkUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {   
                WalkAnimationModifiers(true, UpLeftAlteredwalkUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, DownLeftAlteredwalkDown, defaultSpeedModifier);;
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, walkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(true, DownRightAlteredwalkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, UpRightAlteredwalkUpReversed, speedReductionModifier);
            }
        }
        //walk down right
        else if (velocity.y < -0.5f && velocity.x > 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, UpAlteredwalkUpRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, walkUpLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkLeftUpReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, walkDownLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, DownAlteredwalkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkRightDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, walkUpRight, defaultSpeedModifier);
            }
        }
        //walk right
        else if (velocity.x > 0.5f && velocity.y < 0.5f && velocity.y > -0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(true, UpAlteredwalkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, UpLeftAlteredwalkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, walkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, DownLeftAlteredwalkLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, DownAlteredwalkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(true, DownRightAlteredwalkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, walkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, UpRightAlteredwalkRight, defaultSpeedModifier);
            }
        }
        //walk right up
        else if (velocity.x > 0.5f && velocity.y > 0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(false, UpAlteredwalkUpRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 22.6f && torchRotation.z < 67.5f)
            {
                WalkAnimationModifiers(true, walkUpLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 67.6f && torchRotation.z < 112.5f)
            {
                WalkAnimationModifiers(true, LeftAlteredwalkLeftDownReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(true, walkDownLeftReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 157.6f && torchRotation.z < 202.5f)
            {
                WalkAnimationModifiers(false, DownAlteredwalkDownRightReversed, speedReductionModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(false, RightAlteredwalkRightUp, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(false, walkUpRight, defaultSpeedModifier);
            }
        }
    }

    #endregion

    private void OnStartOrEnable()
    {
        InputManager.InputManagerInstance.onMoveInputChangeEvent += ReactToInput;
    }

    private void OnEnable()
    {
        if (started) OnStartOrEnable();
    }

    private void OnDisable()
    {
        InputManager.InputManagerInstance.onMoveInputChangeEvent -= ReactToInput;
    }
}
