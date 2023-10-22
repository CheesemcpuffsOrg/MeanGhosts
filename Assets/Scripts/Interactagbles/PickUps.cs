using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUps : InteractableObjs
{
    [SerializeField] string graveName; 

    public override void Interact()
    {
        base.Interact();

        
        player.GetComponent<PlayerController>().HeldObject(this.gameObject);

        if(gameObject.transform.parent != null && gameObject.transform.parent.name == graveName)
        {
            GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score - 1;
           // Debug.Log(GameManager.GameManagerInstance.score);
        }

        this.gameObject.SetActive(false);
    }
}
