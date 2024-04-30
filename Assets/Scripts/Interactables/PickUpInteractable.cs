using UnityEngine;

public class PickUpInteractable : InteractableObjs, IInteractable
{
    [SerializeField] string graveName;
    [SerializeField]GameObject ghost;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void InteractionPrompt(bool hasCollided)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        if (ghost == null)
        {
            ghost = GameObject.Find(graveName);
        }
    }

   /* protected override void Interact()
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
         //   SoundManager.SoundManagerInstance.PlayOneShotSound("PickUp");
            string sendThis = null;
            UIContainer.UIContainerInstance.EnableItemImage(sendThis = this.gameObject.name.Replace("(Clone)", string.Empty));

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
    }*/

    /*private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        UIContainer.UIContainerInstance.PickUpItem();
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        UIContainer.UIContainerInstance.PickUpItem();
    }*/
}
