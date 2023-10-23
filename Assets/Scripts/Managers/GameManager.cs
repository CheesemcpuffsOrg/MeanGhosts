using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    GameObject player;

    [Range(0f, 1f)]
    [SerializeField] float lightLevel;

    [SerializeField] int chooseSound;

    /*Camera cam;
    float camHeight, camWidth;*/

    public GameObject[] items;
    [SerializeField] GameObject[] itemSpawnPoints;

    public GameObject[] audioTapes;
    [SerializeField] GameObject[] audioTapesSpawnPoints;

    public int score = 0;

    public float timer;

    bool pause = false;

    private void Awake()
    {
        GameManagerInstance = this;

        player = GameObject.Find("Player");
    }

    private void Start()
    {
        StartCoroutine(RandomAmbientSound()); 

        PlaceItems();

        PlaceAudioTapes();
        
       /* cam = Camera.main;
        cam.aspect = 4 / 3f;
        camHeight = 2 * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;*/
    }

    void Update()
    {
        Darkness.settings.SetDarkness(lightLevel);

        if(score == 6)
        {
            UIManagers.UIManagersInstance.Winner();
            player.GetComponent<PlayerController>().OnDisable();
            player.GetComponent<PlayerController>().flashLight.SetActive(false);
            player.GetComponent<PlayerController>().flashLightState = false;  
        }

        timer += Time.deltaTime;
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

    void PlaceItems()
    {
        int removeItem = 0;
        int removeSpawnPoint = 0;
        GameObject spawnPoint = null;

        foreach (GameObject obj in items)
        {
            if (obj != null)
            {
                for(int i = 0; i < itemSpawnPoints.Length; i++)
                {
                    removeSpawnPoint = Random.Range(0, itemSpawnPoints.Length);
                    if(itemSpawnPoints[removeSpawnPoint] != null)
                    {
                        spawnPoint = itemSpawnPoints[removeSpawnPoint];
                        //Debug.Log(removeSpawnPoint);
                        break;
                    }
                }
                    
                GameObject newObj = Instantiate(obj);
                newObj.transform.position = spawnPoint.transform.position;
                items[removeItem] = null;
                itemSpawnPoints[removeSpawnPoint] = null;
                removeItem += 1; 
            }
        }
    }

    void PlaceAudioTapes()
    {
        int removeItem = 0;
        int removeSpawnPoint = 0;
        GameObject spawnPoint = null; 

        foreach (GameObject obj in audioTapes)
        {
            if (obj != null)
            {
                for (int i = 0; i < audioTapesSpawnPoints.Length; i++)
                {
                    removeSpawnPoint = Random.Range(0, audioTapesSpawnPoints.Length);
                    //Debug.Log(removeSpawnPoint);
                    if (audioTapesSpawnPoints[removeSpawnPoint] != null)
                    {
                        spawnPoint = audioTapesSpawnPoints[removeSpawnPoint];
                        break;
                    }
                }

                GameObject newObj = Instantiate(obj);
                newObj.transform.position = spawnPoint.transform.position;
                audioTapes[removeItem] = null;
                audioTapesSpawnPoints[removeSpawnPoint] = null;
                removeItem += 1;
            }
        }
    }

    public void Pause()
    {
        pause = !pause;

        UIManagers.UIManagersInstance.PauseText();

        if (pause == false)
        {
            Time.timeScale = 1;
            /*player.GetComponent<PlayerController>().flashLight.SetActive(true);
            player.GetComponent<PlayerController>().flashLightState = true;*/
        }
        else
        {
            Time.timeScale = 0;
            player.GetComponent<PlayerController>().flashLight.SetActive(false);
            player.GetComponent<PlayerController>().flashLightState = false;
        }

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        player.GetComponent<PlayerController>().flashLight.SetActive(false);
        player.GetComponent<PlayerController>().flashLightState = false;
        UIManagers.UIManagersInstance.GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
    }
}
