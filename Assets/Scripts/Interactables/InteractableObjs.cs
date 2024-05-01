using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObjs : MonoBehaviour
{
    [SerializeField] GameObject interactableObjType;
    private IInteractable interactable;

    [Header("Collision Proxies")]
    [SerializeField] private Collision2DProxy interactionCollider;

    [Header ("Tags")]
    [SerializeField] TagScriptableObject playerCollision;

    private void Start()
    {
        interactionCollider.OnTriggerEnter2D_Action += InteractionColliderOnTriggerEnter2D;
        interactionCollider.OnTriggerExit2D_Action += InteractionColliderOnTriggerExit2D;

        interactable = interactableObjType.GetComponent<IInteractable>();
    }

    protected void InteractionColliderOnTriggerEnter2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerCollision))
        {
            interactable.InteractionPrompt(true);
            GameplayInputManager.GameplayInputManagerInstance.interactEvent += interactable.Interact;
        }
    }

    protected void InteractionColliderOnTriggerExit2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerCollision))
        {
            interactable.InteractionPrompt(false);
            GameplayInputManager.GameplayInputManagerInstance.interactEvent += interactable.Interact;
        }
    }

    private void OnDisable()
    {
        GameplayInputManager.GameplayInputManagerInstance.interactEvent += interactable.Interact;
        interactable.InteractionPrompt(false);
    }
}
