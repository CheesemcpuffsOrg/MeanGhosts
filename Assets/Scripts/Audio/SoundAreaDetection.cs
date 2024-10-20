using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAreaDetection : MonoBehaviour
{

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    [Header("Colliders")]
    [SerializeField] Collision2DProxy areaCollider;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerCollider;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject areaSound;


    // Start is called before the first frame update
    void Start()
    {
        areaCollider.OnTriggerEnter2D_Action += AreaOnTriggerEnter;
        areaCollider.OnTriggerExit2D_Action += AreaOnTriggerExit;

        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();
    }

    void AreaOnTriggerEnter(Collider2D collider)
    {
        if(TagExtensions.HasTag(collider.gameObject, playerCollider))
        {
            soundComponent.PlaySound(areaSound, transform.position);
        }
    }

    void AreaOnTriggerExit(Collider2D collider)
    {
        if (TagExtensions.HasTag(collider.gameObject, playerCollider))
        {
            soundComponent.StopSound(areaSound);
        }
    }
}
