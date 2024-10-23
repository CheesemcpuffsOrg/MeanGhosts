
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

//Joshua 2023/12/06


public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;

    [HideInInspector] public GameObject player { get; private set; } 

    /*Camera cam;
    float camHeight, camWidth;*/

    public GameObject[] items;
    [SerializeField] GameObject[] itemSpawnPoints;

    public GameObject[] audioTapes;
    [SerializeField] GameObject[] audioTapesSpawnPoints;

    int score = 0;
    public event Action<int> scoreChanged;

    public float timer;

    bool _isPaused = false;
    public bool isPaused => _isPaused;
    

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerTag;

    private void Awake()
    {
        GameManagerInstance = this;
    }

    private void Start()
    {
        player = TagExtensions.FindWithTag(gameObject, playerTag);

        PlaceItems();

        PlaceAudioTapes();

        /* cam = Camera.main;
            cam.aspect = 4 / 3f;
            camHeight = 2 * cam.orthographicSize;
            camWidth = camHeight * cam.aspect;*/
    }

    void Update()
    {
        if (score == 6)
        {
            UIContainer.UIContainerInstance.Winner();
            InputManager.InputManagerInstance.DisablePlayerActions();
            //player.GetComponent<PlayerController>().flashLightObj.SetActive(false);
            // player.GetComponent<Player.PlayerController>().flashLightState = false;
        }

        timer += Time.deltaTime;
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
                for (int i = 0; i < itemSpawnPoints.Length; i++)
                {
                    removeSpawnPoint = Random.Range(0, itemSpawnPoints.Length);
                    if (itemSpawnPoints[removeSpawnPoint] != null)
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

    public void IncreaseScore()
    {
        score++;
        scoreChanged?.Invoke(score);
    }

    public void GameOver()
    {
        InputManager.InputManagerInstance.DisablePlayerActions();
        //player.GetComponent<PlayerController>().flashLightObj.SetActive(false);
        //player.GetComponent<Player.PlayerController>().flashLightState = false;
        UIContainer.UIContainerInstance.GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


