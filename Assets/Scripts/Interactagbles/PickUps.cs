using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PickUps : InteractableObjs
{
    [SerializeField] string graveName;
    [SerializeField]GameObject ghost;

    private void Start()
    {
        if (ghost == null)
        {
            ghost = GameObject.Find(graveName);
        }
    }

    protected override void Interact()
    {
        base.Interact();

        if(player.GetComponent<PlayerController>().heldObject == null)
        {
            SpriteRenderer[] renderers = this.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            this.GetComponent<Collider2D>().enabled = false;
            player.GetComponent<PlayerController>().HeldObject(this.gameObject);
            SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");
            UIManagers.UIManagersInstance.EnableItemImage(this.gameObject.name);

        }

        if(gameObject.transform.parent != null && gameObject.transform.parent.name.Replace("Grave", string.Empty) == graveName)
        {
            GameManager.GameManagerInstance.score = GameManager.GameManagerInstance.score - 1;
            ghost.GetComponent<SpriteRenderer>().enabled = true;
            ghost.GetComponent<Collider2D>().enabled = true;
            Debug.Log(GameManager.GameManagerInstance.score);
        }

        if (this.transform.parent != null)
        {
            this.transform.parent.GetComponent<DropOnGrave>().objectOnGrave = null;
            this.gameObject.transform.parent = null;
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        UIManagers.UIManagersInstance.PickUpItem();
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        UIManagers.UIManagersInstance.PickUpItem();
    }
}
