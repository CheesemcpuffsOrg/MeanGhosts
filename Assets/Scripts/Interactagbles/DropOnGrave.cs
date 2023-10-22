using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DropOnGrave : InteractableObjs
{
    GameObject heldObject;

    GameObject objectOnGrave;

    [SerializeField] string itemName;

    public override void Interact()
    {
        base.Interact();

        heldObject = player.GetComponent<PlayerController>().heldObject;

        if (heldObject != null)
        {
            heldObject.SetActive(true);
            heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            heldObject.transform.parent = this.transform;
            objectOnGrave = heldObject;
            player.GetComponent<PlayerController>().heldObject = null;

           // Debug.Log(objectOnGrave.gameObject.name);

            if(objectOnGrave.gameObject.name == itemName) 
            {
                GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score + 1;
              //  Debug.Log(GameManager.GameManagerInstance.score);
            }
        } 
    }
}
