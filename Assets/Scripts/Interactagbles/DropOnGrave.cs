using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DropOnGrave : InteractableObjs
{
    GameObject heldObject;

    GameObject objectOnGrave;

    [SerializeField] string itemName;

    [SerializeField]GameObject ghost;

    public override void Interact()
    {
        base.Interact();

        heldObject = player.GetComponent<PlayerController>().heldObject;

        if (heldObject != null)
        {
            heldObject.GetComponent<SpriteRenderer>().enabled = true;
            heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 1.5f);
            heldObject.transform.parent = this.transform;
            objectOnGrave = heldObject;
            player.GetComponent<PlayerController>().heldObject = null;
            SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");

            // Debug.Log(objectOnGrave.gameObject.name);

            if (objectOnGrave.gameObject.name == itemName) 
            {
                GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score + 1;
                ghost.SetActive(false);
              //  Debug.Log(GameManager.GameManagerInstance.score);
            }
        } 
    }
}
