using UnityEngine;

namespace Director
{
    public abstract class Sensor:MonoBehaviour
    {
        public abstract SensorResult Evaluate();

        public float MinMaxNormalize()
        {
            return MinMaxNormalize(Evaluate());
        }
        
        public float MinMaxNormalize(SensorResult result)
        {
            return MinMaxNormalize(result.value, result.min, result.max);
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