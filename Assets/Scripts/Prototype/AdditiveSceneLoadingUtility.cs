using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoadingUtility
{
    public async Task LoadSceneAsync(SceneField sceneField)
    {
        try
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneField);

            while (!asyncLoad.isDone)
            {
                await Task.Yield(); // Await until the scene is loaded
            }

            Debug.Log("Scene loaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load the scene: {ex.Message}");
        }
    }

    public async Task UnloadSceneAsync(SceneField sceneField)
    {
        try
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneField);

            while (!asyncLoad.isDone)
            {
                await Task.Yield(); // Await until the scene is loaded
            }

            Debug.Log("Scene unloaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to unload the scene: {ex.Message}");
        }
    }
}
