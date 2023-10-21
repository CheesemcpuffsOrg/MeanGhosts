using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUps : InteractableObjs
{

    public override void Interact()
    {
        base.Interact();

        this.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().HeldObject(this.gameObject);
    }
}
