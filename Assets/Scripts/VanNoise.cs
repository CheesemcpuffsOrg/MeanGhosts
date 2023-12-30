using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanNoise : MonoBehaviour
{
    bool playSound = true;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject bangingSFX;
    [SerializeField] AudioScriptableObject screamSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playSound)
        {
            AudioManager.AudioManagerInstance.PlaySound(bangingSFX, gameObject);
            AudioManager.AudioManagerInstance.PlaySound(screamSFX, gameObject);
            playSound = false;
        }
    }
}
