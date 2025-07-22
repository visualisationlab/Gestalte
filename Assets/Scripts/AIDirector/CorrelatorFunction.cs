using UnityEngine;

namespace AIDirector
{
    public abstract class CorrelatorFunction:ScriptableObject
    {
        public abstract float Evaluate(SensorResult result);
        
        public float MinMaxNormalize(SensorResult result)
        {
            return MinMaxNormalize(result.Value, result.Min, result.Max);
        }
        
        private float MinMaxNormalize(float value, float min, float max)
        {
            if (max == min)
            {
                return 0.0f; // Avoid division by zero
            }
            return (value - min) / (max - min);
        }
    }
}