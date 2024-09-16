using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveSceneController : MonoBehaviour
{

    [SerializeField] List<SceneField> scenesToLoad = new List<SceneField>();

    [SerializeField] Collision2DProxy interactionCollider;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerColliderTag;

    private void Start()
    {
        if (interactionCollider != null)
        {
            interactionCollider.OnTriggerEnter2D_Action += InteractionColliderOnTriggerEnter2D;
        }
    }

    private async void InteractionColliderOnTriggerEnter2D(Collider2D obj)
    {
        if (TagExtensions.HasTag(obj.gameObject, playerColliderTag))
        {
            await AdditiveSceneManager.additiveSceneManagerInstance.LoadScenes(scenesToLoad);
        }
    }
}
