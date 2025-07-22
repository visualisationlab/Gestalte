using UnityEngine;

namespace AIDirector
{
    public abstract class SensorOperation : ScriptableObject
    {
        public abstract float Evaluate(float[] inputs);
    }
}