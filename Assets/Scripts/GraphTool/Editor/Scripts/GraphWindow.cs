
// Editor/Scripts/GraphWindow.cs
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GraphTool.Editor
{
    public class GraphWindow : EditorWindow
    {
        public static GraphWindow Instance { get; private set; }

        void OnEnable() { Instance = this; }
        void OnDisable() { Instance = null; }

        // simple in‑memory store for all series
        static Dictionary<MonoBehaviour, Series> s_Series = new();
        const int kMaxSamples = 300;
        const float kMin = 0, kMax = 1;

        [MenuItem("Window/Graph Tool")]
        public static void ShowWindow()
        {
            GetWindow<GraphWindow>("Graph Tool");
        }

        // Called by your editor‐shim
        public static void Subscribe(MonoBehaviour id, Func<float> provider, Color color)
        {
            Debug.Log($"[Graph] Subscribed “{id.name}”");   // ← make sure this prints
            if (s_Series.ContainsKey(id)) return;
            s_Series[id] = new Series(provider, color, kMaxSamples);
        }

        // Called by your editor‐shim
        public static void Unsubscribe(MonoBehaviour id)
        {
            s_Series.Remove(id);
        }

        public void SampleAll()
        {
            foreach (var kv in s_Series)
                kv.Value.AddSample(kv.Value.Provider(), kMin, kMax);
        }

        void OnGUI()
        {
            const float topMargin    = 10f;
            const float rightMargin  = 10f;
            GUILayout.Space(topMargin);

            const float leftMargin = 40f;
            const float graphHeight  = 200f;
            const float legendHeight = 20f;
            const float padding      = 4f;

            // reserve a rectangle inside the window
            float usableWidth = position.width - leftMargin - rightMargin;
            Rect fullRect = GUILayoutUtility.GetRect(usableWidth, graphHeight + legendHeight + padding);
            
            // Split into label margin + graph + legend area
            Rect labelArea = new Rect(
                fullRect.xMin,
                fullRect.yMin,
                leftMargin,
                graphHeight
            );
            Rect graphRect = new Rect(
                leftMargin,
                topMargin,
                usableWidth,
                graphHeight
            );
            Rect legendRect = new Rect(
                fullRect.xMin + leftMargin,
                fullRect.yMax - legendHeight,
                usableWidth,
                legendHeight
            );
            EditorGUI.DrawRect(graphRect, new Color(0, 0, 0, 0.25f));

            DrawYAxisLabels(graphRect, 5);

            // switch Handles into GUI coordinate space
            Handles.BeginGUI();

            DrawGrid(graphRect);

            // draw each series
            foreach (var kv in s_Series)
            {
                var series = kv.Value;
                // sample once per OnGUI for now
                series.AddSample(series.Provider(), kMin, kMax);

                float[] data = series.Buffer.ToArray();
                if (data.Length < 2) continue;

                Vector3[] pts = new Vector3[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    float t = i / (float)(data.Length - 1);
                    float x = Mathf.Lerp(graphRect.xMin, graphRect.xMax, t);
                    float yNorm = Mathf.InverseLerp(kMin, kMax, data[i]);
                    float y = Mathf.Lerp(graphRect.yMax, graphRect.yMin, yNorm);
                    pts[i] = new Vector3(x, y);
                }

                Handles.color = series.Color;
                Handles.DrawAAPolyLine(2f, pts);
            }

            DrawLegend(graphRect);

            Handles.EndGUI();
        }

        void DrawGrid(Rect graphRect)
        {
                        Handles.color = new Color(1, 1, 1, 0.1f);
            int lines = 10;
            for (int i = 0; i <= lines; i++)
            {
                float x = Mathf.Lerp(graphRect.xMin, graphRect.xMax, i / (float)lines);
                float y = Mathf.Lerp(graphRect.yMin, graphRect.yMax, i / (float)lines);
                Handles.DrawLine(new Vector3(graphRect.xMin, y), new Vector3(graphRect.xMax, y));
                Handles.DrawLine(new Vector3(x, graphRect.yMin), new Vector3(x, graphRect.yMax));
            }

        }

        void DrawYAxisLabels(Rect graphRect, int steps)
        {
            var style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleRight;

            // we want labels at values 0, 25, 50, 75, 100, etc.
            for (int i = 0; i <= steps; i++)
            {
                float t = i/(float)steps;           // 0→1
                float value = Mathf.Lerp(kMin, kMax, t);   // 0→100
                string text = Mathf.RoundToInt(value).ToString();

                // y position: invert t because GUI y grows down
                float y = Mathf.Lerp(graphRect.yMax, graphRect.yMin, t);

                // label rect just to the left of the graph
                var labelRect = new Rect(
                    graphRect.xMin - 40,  // 40px left
                    y - 10,               // center the label
                    36,                   // width
                    20                    // height
                );

                GUI.Label(labelRect, text, style);
            }
        }
        
        void DrawLegend(Rect graphRect)
        {
            const float boxSize = 12f;
            const float padding = 4f;
            float x = graphRect.xMin;
            float y = graphRect.yMax + padding;

            var labelStyle = GUI.skin.label;
            foreach (var kv in s_Series)
            {
                MonoBehaviour id    = kv.Key;
                Color  col   = kv.Value.Color;

                // color swatch
                Rect boxRect = new Rect(x, y, boxSize, boxSize);
                EditorGUI.DrawRect(boxRect, col);

                // label next to it
                Vector2 labelSize = labelStyle.CalcSize(new GUIContent(id.name));
                Rect labelRect    = new Rect(x + boxSize + padding, y, labelSize.x, boxSize);
                GUI.Label(labelRect, id.name, labelStyle);

                // advance x for the next entry
                x += boxSize + padding + labelSize.x + (padding * 2);
            }
        }
    }
}
#endif
