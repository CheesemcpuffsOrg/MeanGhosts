
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FirePitInteractable : InteractableObjs, IInteractable
{

    [Space]
    [Space]
    [Space]
    [Space]

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    [SerializeField] HeldTotems heldTotems;

    [Header("Collision Proxies")]
    [SerializeField] Collision2DProxy audioDetectionCollider;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject fireCrackling;
    [SerializeField] AudioScriptableObject fireWhoosh; 

    protected override void Start()
    {
        base.Start();

        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        audioDetectionCollider.OnTriggerEnter2D_Action += AudioDetectionOntriggerEnter;
        audioDetectionCollider.OnTriggerExit2D_Action += AudioDetectionOntriggerExit;
    }

    public void Interact()
    {
        if (soundComponent.IsSoundPlaying(fireWhoosh) || heldTotems.NumberOfHeldTotems() <= 0)
        {
            return;
        }
        heldTotems.RemoveFirstTotem();
        soundComponent.PlaySound(fireWhoosh, transform.position);
        GameManager.GameManagerInstance.IncreaseScore();
    }

    public void InteractionPrompt(bool hasCollided)
    {
        if(hasCollided)
        {
            Debug.Log("Can Burn");
        }   
    }

    void AudioDetectionOntriggerEnter(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, playerColliderTag))
        {
            if (!soundComponent.IsSoundPlaying(fireCrackling))
            {
                soundComponent.PlaySound(fireCrackling, transform.position);
            }
        }
    }

    void AudioDetectionOntriggerExit(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, playerColliderTag))
        {
            if (soundComponent.IsSoundPlaying(fireCrackling))
            {
                soundComponent.StopSound(fireCrackling);
            }
        }
    }
}
