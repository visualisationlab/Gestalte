using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Name of scene to load (must be in Build Settings)")]
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' can't be loaded. Is it in Build Settings and spelled correctly?");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneFromString(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' can't be loaded. Is it in Build Settings and spelled correctly?");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
