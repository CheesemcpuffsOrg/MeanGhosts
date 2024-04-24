using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    [SerializeField] FlashLight flashLight;
    [SerializeField] PlayerController playerController;

    [Header ("Collision Proxies")]
    [SerializeField] private Collision2DProxy normalBeamCollider;
    [SerializeField] private Collision2DProxy highBeamCollider;
    [SerializeField] private Collision2DProxy ghostDetectionCollider;

    [Header ("Tags")]
    [SerializeField] TagScriptableObject enemyTag;
    [SerializeField] TagScriptableObject spottableByTorch;

    private void Start()
    {
        normalBeamCollider.OnTriggerEnter2D_Action += NormalBeamOnTriggerEnter2D;
        normalBeamCollider.OnTriggerExit2D_Action += NormalBeamOnTriggerExit2D;

        highBeamCollider.OnTriggerEnter2D_Action += HighBeamOnTriggerEnter2D;
        highBeamCollider.OnTriggerExit2D_Action += HighBeamOnTriggerExit2D;

        ghostDetectionCollider.OnTriggerEnter2D_Action += GhostDetectionOnTriggerEnter2D;
        ghostDetectionCollider.OnTriggerExit2D_Action += GhostDetectionOnTriggerExit2D;

    }

    private void NormalBeamOnTriggerEnter2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, spottableByTorch))
        {
            other.GetComponent<CaughtByBeam>().SpottedByTorch(true);
        }
    }

    private void NormalBeamOnTriggerExit2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, spottableByTorch))
        {
            other.GetComponent<CaughtByBeam>().SpottedByTorch(false);
        }
    }

    private void HighBeamOnTriggerEnter2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, spottableByTorch))
        {
            other.GetComponent<CaughtByBeam>().SpottedByHighBeam(true);
        }
    }

    private void HighBeamOnTriggerExit2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, spottableByTorch))
        {
            other.GetComponent<CaughtByBeam>().SpottedByHighBeam(false);
        }
    }

    private void GhostDetectionOnTriggerEnter2D(Collider2D other)
    {
        if(TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            flashLight.IsGhostWithinRange(true);
        }
    }

    private void GhostDetectionOnTriggerExit2D(Collider2D other)
    {
        if (TagExtensions.HasTag(other.gameObject, enemyTag))
        {
            flashLight.IsGhostWithinRange(false);
        }
    }
}
