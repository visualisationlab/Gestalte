using UnityEngine;

namespace Director.Sensors
{
    public class SimpleSensor : Sensor
    {
        public SensorSurface surface;
        public float maxValue = 100.0f;
        public float minValue = 0.0f;
        public string description;

        public override SensorResult Evaluate()
        {
            return new SensorResult
            {
                description = description,
                sensor = this,
                value = surface.Evaluate(),
                max = maxValue,
                min = minValue
            };
        }
    }
}

