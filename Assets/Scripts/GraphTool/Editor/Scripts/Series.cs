
// Editor/Scripts/Series.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraphTool.Editor
{
    internal class Series
    {
        public Func<float>   Provider { get; }
        public Color         Color    { get; }
        public Queue<float>  Buffer   { get; }

        public Series(Func<float> provider, Color color, int capacity)
        {
            Provider = provider;
            Color    = color;
            Buffer   = new Queue<float>(capacity);
        }
        public void AddSample(float value, float min, float max)
        {
            // clamp, enqueue, dequeue oldest if full
            float v = Mathf.Clamp(value, min, max);
            Buffer.Enqueue(v);
            if (Buffer.Count > BufferCapacity)
            {
                Buffer.Dequeue();
            }
        }

        private int BufferCapacity => Buffer is ICollection<float> col ? col.Count : 100;
    }
}
