using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AdditiveSceneManager : MonoBehaviour
{
    [SerializeField] List<SceneField> activeScenes = new List<SceneField>();

    public static AdditiveSceneManager additiveSceneManagerInstance;

    AdditiveSceneLoadingUtility additiveSceneLoadingUtility = new AdditiveSceneLoadingUtility();

    private void Awake()
    {
        additiveSceneManagerInstance = this;
    }

    public async Task LoadScenes(List<SceneField> scenesToLoad)
    {
        foreach(var scene in scenesToLoad) 
        { 
            if (activeScenes.Contains(scene))
            {
                continue;
            }
            else if(!activeScenes.Contains(scene))
            {
                await additiveSceneLoadingUtility.UnloadSceneAsync(scene);
                activeScenes.Remove(scene);
            }
            else
            {
                await additiveSceneLoadingUtility.LoadSceneAsync(scene);
                activeScenes.Add(scene);
            }
        }
    }
}
