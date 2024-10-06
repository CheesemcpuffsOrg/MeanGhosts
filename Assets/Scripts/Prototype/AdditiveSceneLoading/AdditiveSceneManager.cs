using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{

    private class ActiveScenesMapping
    {
        SceneField scene;
        public SceneField Scene => scene;

        int count;
        public int Count => count;

        public ActiveScenesMapping(SceneField scene)
        {
            this.scene = scene;
            count++;
        }

        public void IncreaseCount()
        {
            count++;
        }

        public void DecreaseCount()
        {
            count--;
        }
    }

    enum LoadingType
    {
        none,
        load,
        unload
    }

    [SerializeField] SceneField startScene;

    public static AdditiveSceneManager additiveSceneManagerInstance;

    // Queue for individual scene loading/unloading requests
    private Queue<SceneField> sceneLoadQueue = new Queue<SceneField>();
    private Queue<SceneField> sceneUnloadQueue = new Queue<SceneField>();

    // Semaphore to ensure only one scene loading/unloading operation at a time
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    private bool isProcessingQueue = false;

    List<ActiveScenesMapping> activeScenesMapping = new List<ActiveScenesMapping>();

    AdditiveSceneLoadingUtility additiveSceneLoadingUtility = new AdditiveSceneLoadingUtility();

    private void Awake()
    {
        additiveSceneManagerInstance = this;

        LoadFirstScene();
    }

    private async void LoadFirstScene()
    {
        await additiveSceneLoadingUtility.LoadSceneAsync(startScene, LoadSceneMode.Additive);
        activeScenesMapping.Add(new ActiveScenesMapping(startScene));
        activeScenesMapping[0].DecreaseCount(); //don't count the first scene that is generated;

    }

    public async Task LoadSceneRequest(SceneField sceneToLoad)
    {
        // Add the scene to the load queue
        sceneLoadQueue.Enqueue(sceneToLoad);

        // Ensure only one scene is processed at a time
        if (!isProcessingQueue)
        {
            await ProcessQueueAsync(LoadingType.load);
        }
    }

    public async Task UnloadSceneRequest(SceneField sceneToUnload)
    {
        // Add the scene to the unload queue
        sceneUnloadQueue.Enqueue(sceneToUnload);

        // Ensure only one scene is processed at a time
        if (!isProcessingQueue)
        {
            await ProcessQueueAsync(LoadingType.unload);
        }
    }

    private async Task ProcessQueueAsync(LoadingType loadingType)
    {
        isProcessingQueue = true;
        await semaphore.WaitAsync(); // Ensure only one process runs at a time

        try
        {
            if (loadingType == LoadingType.load)
            {
                while (sceneLoadQueue.Count > 0)
                {
                    var sceneToLoad = sceneLoadQueue.Dequeue();
                    await LoadScene(sceneToLoad);
                }
            }
            else if (loadingType == LoadingType.unload)
            {
                while (sceneUnloadQueue.Count > 0)
                {
                    var sceneToUnload = sceneUnloadQueue.Dequeue();
                    await UnloadScene(sceneToUnload);
                }
            }
        }
        finally
        {
            semaphore.Release();
            isProcessingQueue = false; // Allow the next operation
        }
    }

    private async Task LoadScene(SceneField sceneToLoad)
    {
        bool isSceneAlreadyLoaded = false;

        // Check if the scene is already loaded
        foreach (var mapping in activeScenesMapping)
        {
            if (mapping.Scene.Equals(sceneToLoad))
            {
                mapping.IncreaseCount();
                isSceneAlreadyLoaded = true;
                break;
            }
        }

        if (!isSceneAlreadyLoaded)
        {
            await additiveSceneLoadingUtility.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            activeScenesMapping.Add(new ActiveScenesMapping(sceneToLoad));
        }
    }

    private async Task UnloadScene(SceneField sceneToUnload)
    {
        // Check if the scene is already loaded
        for (int i = activeScenesMapping.Count - 1; i >= 0; i--)
        {
            var mapping = activeScenesMapping[i];

            if (!mapping.Scene.Equals(sceneToUnload)) continue;

            mapping.DecreaseCount();

            if (mapping.Count <= 0)
            {
                await additiveSceneLoadingUtility.UnloadSceneAsync(sceneToUnload);
                activeScenesMapping.RemoveAt(i);
            }
            break;
        }
    }
}
