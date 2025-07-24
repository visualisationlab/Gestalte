// Editor/Scripts/GraphSubscriptionEditor.cs
#if UNITY_EDITOR
using GraphTool.Runtime;
using UnityEditor;

namespace GraphTool.Editor
{
    [InitializeOnLoad]
    internal static class GraphSubscriptionEditor
    {
        static GraphSubscriptionEditor()
        {
            // Whenever runtime code calls GraphSubscription.Subscribe,
            // forward it to GraphWindow.Subscribe:
            GraphSubscription.OnSubscribe   += GraphWindow.Subscribe;
            GraphSubscription.OnUnsubscribe += GraphWindow.Unsubscribe;
        }
    }
}
#endif
