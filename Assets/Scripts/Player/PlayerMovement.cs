using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] AnimationController animController;
    [SerializeField] PlayerSO playerSO;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] GameObject flashLightObj;

    Vector2 currentMoveInput;
    Vector2 smoothCurrentMoveInput;
    Vector2 currentMovementSmoothVelocity;
    float currentSpeed;
    float defaultSpeed;
    float defaultSpeedModifier = 1f;
    [SerializeField] float speedReductionModifier = 0.5f;

    bool isMoving = false;
    bool footsteps = false;

    [Header("Animation")]
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

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject footSteps;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = playerSO.speed;
        defaultSpeed = playerSO.speed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayFootsteps();

        if (isMoving)
        {
            WalkAnimation();
        }
        else
        {
            animController.PlayAnimation(idle);
        }
    }

    private void FixedUpdate()
    {
        currentMoveInput = controller.currentMoveInput;

        if (currentMoveInput.y != 0 || currentMoveInput.x != 0) { isMoving = true; } else { isMoving = false; }

        SmoothMovement();
    }

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

    #region --WALK ANIMATION --
    void WalkAnimation()
    {
        var torchRotation = flashLightObj.transform.rotation.eulerAngles;
        var velocity = rb.velocity;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        //walk up
        if (velocity.y > 0.5f && velocity.x > -0.5 && velocity.x < 0.5f)
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
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, true, walkLeft, defaultSpeedModifier);
            }
            else if (torchRotation.z > 112.6f && torchRotation.z < 157.5f)
            {
                WalkAnimationModifiers(spriteRenderer, true, walkDownLeft, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, defaultSpeedModifier);
            }
        }
        //walk left
        else if (velocity.x < -0.5f && velocity.y > -0.5 && velocity.y < 0.5f)
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
                WalkAnimationModifiers(spriteRenderer, false, walkDown, defaultSpeedModifier);
            }
            else if (torchRotation.z > 202.6f && torchRotation.z < 247.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, false, walkDownRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 247.6f && torchRotation.z < 292.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, false, walkRight, defaultSpeedModifier);
            }
            else if (torchRotation.z > 292.6f && torchRotation.z < 337.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUpRight, defaultSpeedModifier);
            }
        }
        //walk right
        else if (velocity.x > 0.5f && velocity.y < 0.5f && velocity.y > -0.5f)
        {
            if (torchRotation.z > 337.6f || torchRotation.z < 22.5f)
            {
                WalkAnimationModifiers(spriteRenderer, false, walkUp, defaultSpeedModifier);
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
                WalkAnimationModifiers(spriteRenderer, true, walkUpLeft, defaultSpeedModifier);
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
    }

    private void WalkAnimationModifiers(SpriteRenderer spriteRenderer, bool flipSprite, string animationName, float speedMultiplier)
    {
        spriteRenderer.flipX = flipSprite;
        animController.PlayAnimation(animationName);
        currentSpeed = defaultSpeed * speedMultiplier;
    }

    #endregion
}
