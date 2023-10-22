using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjs : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().interactEvent.AddListener(Interact);
            player = other.gameObject;
            player.GetComponent<PlayerController>().canInteract = true; 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().interactEvent.RemoveListener(Interact);
            player.GetComponent<PlayerController>().canInteract =false;
        }
    }

    public virtual void Interact()
    {
        //this is inherited
       // Debug.Log("Interact");
    }
}
