using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FirePitInteractable : InteractableObjs, IInteractable
{
    /*[SerializeField]GameObject heldObject;

    public GameObject objectOnGrave;

    [SerializeField] string itemName;

    [SerializeField]GameObject ghost;*/

    // bool standingOnGrave;
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
            return;
        }
        heldTotems.RemoveFirstTotem();
        AudioManager.AudioManagerInstance.PlaySound(fireWhoosh, gameObject);  
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



    /* private void Update()
     {
         if(player != null)
         {
             if (standingOnGrave && player.GetComponent<PlayerController>().heldObject != null && objectOnGrave == null)
             {
                 UIContainer.UIContainerInstance.PlaceItemShowText();
             }
         }

     }

     protected override void Interact()
     {
         base.Interact();

         heldObject = player.GetComponent<PlayerController>().heldObject;


         if (heldObject != null && objectOnGrave == null)
         {
             UIContainer.UIContainerInstance.PlaceItemHideText();
             SpriteRenderer[] renderers = heldObject.GetComponentsInChildren<SpriteRenderer>();
             foreach (SpriteRenderer renderer in renderers)
             {
                 renderer.enabled = true;
             }
             heldObject.GetComponent<Collider2D>().enabled = true;
             heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 1.5f);
             heldObject.transform.parent = this.transform;
             objectOnGrave = heldObject;
             string objectOnGraveName = objectOnGrave.name.Replace("(Clone)", string.Empty);
           //  SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");
             player.GetComponent<PlayerController>().heldObject = null;
             UIContainer.UIContainerInstance.DisableItemImage();


             // Debug.Log(objectOnGrave.gameObject.name);

             if (objectOnGraveName == itemName) 
             {
                 GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score + 1;
                 ghost.GetComponent<SpriteRenderer>().enabled = false;
                 ghost.GetComponent<Collider2D>().enabled = false;
                 //  Debug.Log(GameManager.GameManagerInstance.score);
             }
         } 
     }

     private new void OnTriggerEnter2D(Collider2D collision)
     {
         base.OnTriggerEnter2D(collision);

         UIContainer.UIContainerInstance.GraveName(ghost.name);
         player.GetComponent<PlayerController>().canInteract = true;
         standingOnGrave = true; ;

     }

     private new void OnTriggerExit2D(Collider2D collision)
     {
         base.OnTriggerExit2D(collision);

         UIContainer.UIContainerInstance.GraveName(ghost.name);
         if (player.GetComponent<PlayerController>().heldObject != null && objectOnGrave == null)
         {
             UIContainer.UIContainerInstance.PlaceItemHideText();
         }
         player.GetComponent<PlayerController>().canInteract = false;
         standingOnGrave = false;
 }*/
}