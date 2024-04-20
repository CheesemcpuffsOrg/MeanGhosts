using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioScriptableObject crickets;
    [SerializeField] AudioScriptableObject cicadas;
    [SerializeField] AudioScriptableObject wind;


    private void Start()
    {
        AudioManager.AudioManagerInstance.PlaySound(crickets, this.gameObject);
        AudioManager.AudioManagerInstance.PlaySound(cicadas, this.gameObject);
        AudioManager.AudioManagerInstance.PlaySound(wind, this.gameObject);
    }
}
