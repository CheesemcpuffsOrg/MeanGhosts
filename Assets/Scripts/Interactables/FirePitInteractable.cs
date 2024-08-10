using AudioSystem;
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
    [SerializeField] HeldTotems heldTotems;

    [Header("Collision Proxies")]
    [SerializeField] Collision2DProxy audioDetectionCollider;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject fireCrackling;
    [SerializeField] AudioScriptableObject fireWhoosh; 

    protected override void Start()
    {

        base.Start();

        audioDetectionCollider.OnTriggerEnter2D_Action += AudioDetectionOntriggerEnter;
        audioDetectionCollider.OnTriggerExit2D_Action += AudioDetectionOntriggerExit;
    }

    public void Interact()
    {
        if (AudioManager.AudioManagerInstance.IsSoundPlaying(fireWhoosh, this.gameObject) || heldTotems.NumberOfHeldTotems() <= 0)
        {
            Debug.Log("No held totems");
            return;
        }
        heldTotems.RemoveFirstTotem();
        AudioManager.AudioManagerInstance.PlaySound(fireWhoosh, gameObject);
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
            if (!AudioManager.AudioManagerInstance.IsSoundPlaying(fireCrackling, this.gameObject))
            {
                Debug.Log("play audio");
                AudioManager.AudioManagerInstance.PlaySound(fireCrackling, gameObject);
            }
        }
    }

    void AudioDetectionOntriggerExit(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, playerColliderTag))
        {
            if (AudioManager.AudioManagerInstance.IsSoundPlaying(fireCrackling, this.gameObject))
            {
                Debug.Log("stop audio");
                AudioManager.AudioManagerInstance.StopSound(fireCrackling, gameObject);
            }
        }
    }
}
