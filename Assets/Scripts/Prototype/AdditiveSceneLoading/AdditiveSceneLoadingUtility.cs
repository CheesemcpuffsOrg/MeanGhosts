using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoadingUtility
{
    public async Task LoadSceneAsync(SceneField sceneField, LoadSceneMode loadSceneMode)
    {
        try
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneField, loadSceneMode);

            while (!asyncLoad.isDone)
            {
                await Task.Yield(); // Await until the scene is loaded
            }

            //Debug.Log($"Scene {sceneField.SceneName} loaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load the scene: {ex.Message}");
        }
    }

    /// <summary>
    /// Unload scene.
    /// </summary>
    public async Task UnloadSceneAsync(SceneField sceneField)
    {
        var scene = SceneManager.GetSceneByName(sceneField);

        if (scene.isLoaded)
        {
            try
            {
                AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneField);

                while (!asyncLoad.isDone)
                {
                    await Task.Yield(); // Await until the scene is loaded
                }

                //Debug.Log($"Scene {sceneField.SceneName} unloaded successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to unload the scene: {ex.Message}");
            }
        }
        else
        {
            Debug.Log(sceneField.SceneName + " is not loaded, no need to unload.");
        }
        
    }

    /// <summary>
    /// Call this to unload assets.
    /// Make sure to call it after you have loaded a new scene so you aren't unloading assets that are in the new scene to then load them again.
    /// </summary>
    /// <returns></returns>
    public async Task UnloadAssetsAsync()
    {
        try
        {
            AsyncOperation asyncLoad = Resources.UnloadUnusedAssets();

            while (!asyncLoad.isDone)
            {
                await Task.Yield(); // Await until the scene is loaded
            }

            //Debug.Log("Assets unloaded successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to unload assets: {ex.Message}");
        }
    }
}
