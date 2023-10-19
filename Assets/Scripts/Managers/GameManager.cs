using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    [Range(0f, 1f)]
    [SerializeField] float lightLevel;

    [SerializeField] int chooseSound;

    Camera cam;
    float camHeight, camWidth;

    private void Start()
    {
        StartCoroutine(RandomAmbientSound()); 
        
        cam = Camera.main;
        cam.aspect = 4 / 3f;
        camHeight = 2 * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    void Update()
    {
        Darkness.settings.SetDarkness(lightLevel);
    }

    IEnumerator RandomAmbientSound()
    {
        yield return new WaitForSeconds(Random.Range(15f, 30f));

        chooseSound = Random.Range(1, 101);

        if(chooseSound > 0 && chooseSound < 33)
        {
            SoundManager.SoundManagerInstance.PlayOneShotSound("Owl");
        }
        else if (chooseSound > 32 && chooseSound < 65)
        {
            SoundManager.SoundManagerInstance.PlayOneShotSound("Crickets");
        }
        else if (chooseSound > 64 && chooseSound < 97)
        {
            SoundManager.SoundManagerInstance.PlayOneShotSound("Frog");
        }
        else if(chooseSound > 96 && chooseSound < 101)
        {
            SoundManager.SoundManagerInstance.PlayOneShotSound("BloodScream");
        }
        

        StartCoroutine(RandomAmbientSound());
    }
}
