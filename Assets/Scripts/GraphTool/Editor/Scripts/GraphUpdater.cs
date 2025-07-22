// Editor/Scripts/GraphUpdater.cs
#if UNITY_EDITOR
using UnityEditor;
using GraphTool.Editor;   // adjust to match your namespace

[InitializeOnLoad]
internal static class GraphUpdater
{
    static GraphUpdater()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        // if the window exists, sample & repaint it every editor‑frame
        if (GraphWindow.Instance != null)
        {
            GraphWindow.Instance.SampleAll();
            GraphWindow.Instance.Repaint();
        }
    }
}
#endif
