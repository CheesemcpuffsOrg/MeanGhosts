using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{

    [SerializeField] SceneField startScene;

    List<SceneField> activeScenes = new List<SceneField>();

    public static AdditiveSceneManager additiveSceneManagerInstance;

    AdditiveSceneLoadingUtility additiveSceneLoadingUtility = new AdditiveSceneLoadingUtility();

    private void Awake()
    {
        additiveSceneManagerInstance = this;

        LoadFirstScene();
    }

    private async void LoadFirstScene()
    {
        await additiveSceneLoadingUtility.LoadSceneAsync(startScene, LoadSceneMode.Additive);
        activeScenes.Add(startScene);
    }

    public async Task LoadScenes(List<SceneField> scenesToLoad)
    {
        // Unload scenes that are no longer needed
        var scenesToUnload = activeScenes.Where(scene => !scenesToLoad.Contains(scene)).ToList();

        foreach (var scene in scenesToUnload)
        {
            await additiveSceneLoadingUtility.UnloadSceneAsync(scene);
            activeScenes.Remove(scene);
        }

        // Load scenes that are not already loaded
        foreach (var scene in scenesToLoad)
        {
            if (!activeScenes.Contains(scene))
            {
                await additiveSceneLoadingUtility.LoadSceneAsync(scene, LoadSceneMode.Additive);
                activeScenes.Add(scene);
            }
        }
    }
}
