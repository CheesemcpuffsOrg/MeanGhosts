using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnGrave : InteractableObjs
{
    GameObject heldObject;

    GameObject objectOnGrave;

    public override void Interact()
    {
        base.Interact();

        heldObject = player.GetComponent<PlayerController>().heldObject;

        if (heldObject != null)
        {
            heldObject.SetActive(true);
            heldObject.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            objectOnGrave = heldObject;
            player.GetComponent<PlayerController>().heldObject = null;
        } 
    }
}
