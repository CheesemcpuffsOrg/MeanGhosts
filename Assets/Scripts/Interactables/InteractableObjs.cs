using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObjs : MonoBehaviour
{
    private IInteractable interactable;

    [Header("Inherited Collision Proxies")]
    [SerializeField] protected Collision2DProxy interactionCollider;

    [Header ("Inherited Tags")]
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
            InputManager.InputManagerInstance.interactEvent += interactable.Interact;
        }
    }

    private void InteractionColliderOnTriggerExit2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            interactable.InteractionPrompt(false);
            InputManager.InputManagerInstance.interactEvent -= interactable.Interact;
        }
    }

    protected virtual void OnDisable()
    {
        InputManager.InputManagerInstance.interactEvent -= interactable.Interact;
        interactable.InteractionPrompt(false);
    }

    protected virtual void OnDestroy()
    {
        InputManager.InputManagerInstance.interactEvent -= interactable.Interact;
        interactable.InteractionPrompt(false);
    }
}
