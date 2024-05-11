using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObjs : MonoBehaviour
{
    private IInteractable interactable;

    [Header("Parent Collision Proxies")]
    [SerializeField] protected Collision2DProxy interactionCollider;

    [Header ("Parent Tags")]
    [SerializeField] protected TagScriptableObject playerColliderTag;

    protected virtual void Start()
    {
        if(interactionCollider != null)
        {
            interactionCollider.OnTriggerEnter2D_Action += InteractionColliderOnTriggerEnter2D;
            interactionCollider.OnTriggerExit2D_Action += InteractionColliderOnTriggerExit2D;
        }
        
        interactable = this.gameObject.GetComponent<IInteractable>();
    }

    private void InteractionColliderOnTriggerEnter2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            interactable.InteractionPrompt(true);
        }
    }

    private void InteractionColliderOnTriggerExit2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            interactable.InteractionPrompt(false);
            GameplayInputManager.GameplayInputManagerInstance.interactEvent -= interactable.Interact;
        }
    }

    protected virtual void OnDisable()
    {
        GameplayInputManager.GameplayInputManagerInstance.interactEvent -= interactable.Interact;
        interactable.InteractionPrompt(false);
    }
}
