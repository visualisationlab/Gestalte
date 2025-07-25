// Runtime/Scripts/GraphSubscription.cs
using System;
using UnityEngine;

namespace GraphTool.Runtime
{
    public static class GraphSubscription
    {
        // These events are only ever hooked by Editor code.
        public static event Action<MonoBehaviour, Func<float>, Color> OnSubscribe;
        public static event Action<MonoBehaviour>                   OnUnsubscribe;

        /// <summary>
        /// Game code calls this every time you want to start plotting a series.
        /// </summary>
        public static void Subscribe(MonoBehaviour id, Func<float> provider, Color color)
        {
#if UNITY_EDITOR
            OnSubscribe?.Invoke(id, provider, color);
#endif
        }

        /// <summary>
        /// Game code calls this to stop plotting that series.
        /// </summary>
        public static void Unsubscribe(MonoBehaviour id)
        {
#if UNITY_EDITOR
            OnUnsubscribe?.Invoke(id);
#endif
        }
    }
}
