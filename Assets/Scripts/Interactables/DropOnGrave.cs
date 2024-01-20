using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DropOnGrave : InteractableObjs
{
    [SerializeField]GameObject heldObject;

    public GameObject objectOnGrave;

    [SerializeField] string itemName;

    [SerializeField]GameObject ghost;

    bool standingOnGrave;

    private void Update()
    {
        if(player != null)
        {
            if (standingOnGrave && player.GetComponent<PlayerController>().heldObject != null && objectOnGrave == null)
            {
                UIManagers.UIManagersInstance.PlaceItemShowText();
            }
        }
        
    }

    protected override void Interact()
    {
        base.Interact();

        heldObject = player.GetComponent<PlayerController>().heldObject;
        

        if (heldObject != null && objectOnGrave == null)
        {
            UIManagers.UIManagersInstance.PlaceItemHideText();
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
            UIManagers.UIManagersInstance.DisableItemImage();
            

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

        UIManagers.UIManagersInstance.GraveName(ghost.name);
        player.GetComponent<PlayerController>().canInteract = true;
        standingOnGrave = true; ;

    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        UIManagers.UIManagersInstance.GraveName(ghost.name);
        if (player.GetComponent<PlayerController>().heldObject != null && objectOnGrave == null)
        {
            UIManagers.UIManagersInstance.PlaceItemHideText();
        }
        player.GetComponent<PlayerController>().canInteract = false;
        standingOnGrave = false;
    }
}
