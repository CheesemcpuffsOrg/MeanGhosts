using AudioSystem;
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

    public ControlScheme controlScheme;

    GameObject player;

    /*Camera cam;
    float camHeight, camWidth;*/

    public GameObject[] items;
    [SerializeField] GameObject[] itemSpawnPoints;

    public GameObject[] audioTapes;
    [SerializeField] GameObject[] audioTapesSpawnPoints;

    public int score = 0;
    public float timer;

    bool pause = false;

    [Header("Sounds")]
    [SerializeField] List<ObjectPool<AudioScriptableObject>> randomSoundPool;

    [Header("Tags")]
    [SerializeField] TagScriptableObject playerTag;

    private void Awake()
    {
        GameManagerInstance = this;

        controlScheme = new ControlScheme();
        controlScheme.Pause.Pause.performed += Pause;

        player = TagExtensions.FindWithTag(gameObject, playerTag);
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
        if (score == 6)
        {
            UIManagers.UIManagersInstance.Winner();
            player.GetComponent<PlayerController>().OnDisable();
            player.GetComponent<PlayerController>().flashLight.SetActive(false);
            // player.GetComponent<Player.PlayerController>().flashLightState = false;
        }

        timer += Time.deltaTime;
    }

    IEnumerator RandomAmbientSound()
    {
        yield return new WaitForSeconds(Random.Range(15f, 30f));

        AudioScriptableObject soundToPlay = (AudioScriptableObject)RandomUtility.ObjectPoolCalculator(randomSoundPool);

        AudioManager.AudioManagerInstance.PlaySound(soundToPlay, this.gameObject);

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

    void Pause(InputAction.CallbackContext pauseGame)
    {
        pause = !pause;

        UIManagers.UIManagersInstance.PauseText();

        if (pause == false)
        {
            Time.timeScale = 1;
            player.GetComponentInChildren<FlashLight>().Pause(pause);
            player.GetComponent<PlayerController>().OnEnable();
        }
        else
        {
            Time.timeScale = 0;
            player.GetComponentInChildren<FlashLight>().Pause(pause);
            player.GetComponent<PlayerController>().OnDisable();
        }

    }

    public void GameOver()
    {
        player.GetComponent<PlayerController>().OnDisable();
        player.GetComponent<PlayerController>().flashLight.SetActive(false);
        //player.GetComponent<Player.PlayerController>().flashLightState = false;
        UIManagers.UIManagersInstance.GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnEnable()
    {
        controlScheme.Pause.Enable();
    }

    void OnDisable()
    {
        controlScheme.Pause.Disable();
    }
}


