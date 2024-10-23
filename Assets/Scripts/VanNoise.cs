
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanNoise : MonoBehaviour
{

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    bool playSound = true;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject bangingSFX;
    [SerializeField] AudioScriptableObject screamSFX;

    private void Start()
    {
        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playSound)
        {
            soundComponent.PlaySound(bangingSFX, transform.position);
            soundComponent.PlaySound(screamSFX, transform.position);
            playSound = false;
        }
    }
}
