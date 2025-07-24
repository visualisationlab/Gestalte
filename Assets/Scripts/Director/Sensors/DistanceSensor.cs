using UnityEngine;

namespace Director.Sensors
{
    public class DistanceSensor : Sensor
    {
        public Transform other;
        public float maxDistance = 100.0f;
        public string description;
        public override SensorResult Evaluate()
        {
            if (other == null)
            {
                Debug.LogError("DistanceSensor: other Transform is not assigned.");
                return new SensorResult
                {
                    description = description,
                    sensor = this, 
                    value = float.NaN,
                    max = maxDistance,
                    min = 0.0f
                };
            }

            return new SensorResult
            {
                description = description,
                sensor = this,
                value = Vector3.Distance(transform.position, other.position),
                max = maxDistance,
                min = 0.0f
            };
        }
    }
}