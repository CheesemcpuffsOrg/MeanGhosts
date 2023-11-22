using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObjs : MonoBehaviour
{
    public GameObject player;

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //other.GetComponent<PlayerController>().interactEvent.AddListener();
            player = other.gameObject;
            
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player.PlayerController>().interactEvent.RemoveListener(Interact);
            
        }
    }

    protected virtual void Interact()
    {
        //this is inherited
       // Debug.Log("Interact");
    }
}
