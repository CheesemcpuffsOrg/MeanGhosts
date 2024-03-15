using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioScriptableObject ambientSound;
    [SerializeField] float minSeconds = 15f;
    [SerializeField] float maxSeconds = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomAmbientSound());
    }

    IEnumerator RandomAmbientSound()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(minSeconds, maxSeconds));

            AudioManager.AudioManagerInstance.PlaySound(ambientSound, this.gameObject);
        }
        
    }
}
