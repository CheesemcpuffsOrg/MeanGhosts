using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUps : InteractableObjs
{
    [SerializeField] string graveName;
    GameObject ghost;

    private void Start()
    {
        if (ghost == null)
        {
            ghost = GameObject.Find(graveName);
        }
    }

    public override void Interact()
    {
        base.Interact();

        if(player.GetComponent<PlayerController>().heldObject == null)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            player.GetComponent<PlayerController>().HeldObject(this.gameObject);
            SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");
        }

        if(gameObject.transform.parent != null && gameObject.transform.parent.name == graveName)
        {
            GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score - 1;
            ghost.gameObject.SetActive(true);
           // Debug.Log(GameManager.GameManagerInstance.score);
        }

        
    }
}
