using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Name of scene to load (must be in Build Settings)")]
    [SerializeField] private string sceneName;

    public void CallLoadGame()
    {
        StartCoroutine(LoadSceneDelayedAsync(sceneName, 1f));
    }

    private IEnumerator LoadSceneDelayedAsync(string sceneName, float delay)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' can't be loaded. Is it in Build Settings and spelled correctly?");
            yield break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false; // hold activation

        // Optional: you can surface op.progress (goes 0 to 0.9) to UI here.

        float timer = 0f;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // After the delay, allow the scene to activate.
        op.allowSceneActivation = true;

        // Wait until the operation actually finishes.
        yield return op;
    }
}
