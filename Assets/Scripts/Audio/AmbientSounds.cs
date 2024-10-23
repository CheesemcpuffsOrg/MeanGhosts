
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    [Header("Sounds")]
    [SerializeField] AudioScriptableObject crickets;
    [SerializeField] AudioScriptableObject cicadas;
    [SerializeField] AudioScriptableObject wind;


    private void Start()
    {
        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        soundComponent.PlaySound(crickets, transform.position);
        soundComponent.PlaySound(cicadas, transform.position);
        soundComponent.PlaySound(wind, transform.position);
    }
}
