using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBeam : MonoBehaviour
{

    [SerializeField] TagScriptableObject enemyTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(TagExtensions.HasTag(collision.gameObject, enemyTag))
        {
            collision.GetComponentInChildren<AIStateManager>().SpottedByTorch(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TagExtensions.HasTag(collision.gameObject, enemyTag))
        {
            collision.GetComponentInChildren<AIStateManager>().SpottedByTorch(false);
        }
    }
}
