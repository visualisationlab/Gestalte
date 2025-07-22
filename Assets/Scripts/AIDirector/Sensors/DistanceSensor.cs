using System.Collections.Generic;
using UnityEngine;

namespace AIDirector.Sensors
{
    public class DistanceSensor : Sensor
    {
        public Transform other;
        public float maxDistance = 100.0f;
        
        public override SensorResult Evaluate()
        {
            if (other == null)
            {
                Debug.LogError("DistanceSensor: other Transform is not assigned.");
                return new SensorResult
                {
                    Sensor = this, 
                    Value = float.NaN,
                    Max = maxDistance,
                    Min = 0.0f,
                    Inputs = new List<SensorResult>()
                };
            }

            return new SensorResult
            {
                Sensor = this,
                Value = Vector3.Distance(transform.position, other.position),
                Max = maxDistance,
                Min = 0.0f,
                Inputs = new List<SensorResult>()
            };
        }
    }
}