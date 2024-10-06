using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveSceneController : MonoBehaviour
{
    [SerializeField] List<SceneField> scenesToLoad = new List<SceneField>();

    [SerializeField] Collision2DProxy interactionCollider;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerColliderTag;

    private bool isPlayerInside = false;

    private void Start()
    {
        if (interactionCollider != null)
        {
            interactionCollider.OnTriggerEnter2D_Action += InteractionColliderOnTriggerEnter2D;
            interactionCollider.OnTriggerExit2D_Action += InteractionColliderOnTriggerExit2D;
        }
    }

    private async void InteractionColliderOnTriggerEnter2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            if (isPlayerInside) return; // Debounce: If already inside, do nothing
            isPlayerInside = true;

            foreach (var scene in scenesToLoad)
            {
                await AdditiveSceneManager.additiveSceneManagerInstance.LoadSceneRequest(scene);
            }
        }
    }

    private async void InteractionColliderOnTriggerExit2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            if (!isPlayerInside) return; // Debounce: If already outside, do nothing
            isPlayerInside = false;

            foreach (var scene in scenesToLoad)
            {
                await AdditiveSceneManager.additiveSceneManagerInstance.UnloadSceneRequest(scene);
            }
        }
    }
}
