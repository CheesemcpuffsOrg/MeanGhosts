using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioScriptableObject ambientSound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomAmbientSound());
    }

    IEnumerator RandomAmbientSound()
    {
        yield return new WaitForSeconds(Random.Range(15f, 30f));

        AudioManager.AudioManagerInstance.PlaySound(ambientSound, this.gameObject);

        StartCoroutine(RandomAmbientSound());
    }
}
