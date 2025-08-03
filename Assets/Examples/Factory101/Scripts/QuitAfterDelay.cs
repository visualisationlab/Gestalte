using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitAfterDelay : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 4f;

    void Start()
    {
        Invoke(nameof(QuitGame), delaySeconds);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        // Stops play mode in the editor
        EditorApplication.isPlaying = false;
#else
        // Quits the built application
        Application.Quit();
#endif
    }
}
