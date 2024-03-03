using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [Header ("Collision Proxies")]
    [SerializeField] private Collision2DProxy normalBeamCollider;
    [SerializeField] private Collision2DProxy highBeamCollider;  

    [Header ("Tags")]
    [SerializeField] TagScriptableObject enemyTag;

    FlashLight flashLight;

    private void Start()
    {
        flashLight = this.gameObject.GetComponent<FlashLight>();

        CreateTriggers();
    }

    private void CreateTriggers()
    {
        normalBeamCollider.OnTriggerEnter2D_Action += NormalBeamOnTriggerEnter2D;
        normalBeamCollider.OnTriggerExit2D_Action += NormalBeamOnTriggerExit2D;

        highBeamCollider.OnTriggerEnter2D_Action += HighBeamOnTriggerEnter2D;
        highBeamCollider.OnTriggerExit2D_Action += HighBeamOnTriggerExit2D;
    }

    private void NormalBeamOnTriggerEnter2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            other.GetComponent<CaughtByBeam>().SpottedByTorch(true, flashLight);
        }
    }

    private void NormalBeamOnTriggerExit2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            other.GetComponent<CaughtByBeam>().SpottedByTorch(false, flashLight);
        }
    }

    private void HighBeamOnTriggerEnter2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            other.GetComponent<CaughtByBeam>().SpottedByHighBeam(true, flashLight);
        }
    }

    private void HighBeamOnTriggerExit2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            other.GetComponent<CaughtByBeam>().SpottedByHighBeam(false, flashLight);
        }
    }
}
