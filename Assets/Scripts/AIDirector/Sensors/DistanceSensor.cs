using System.Collections.Generic;
using UnityEngine;

namespace AIDirector.Sensors
{
    public class DistanceSensor : Sensor
    {
        public SensorOperation evalFunc;
        public Transform a;
        public Transform b;
        public float bias = 1.0f;
        public float maxDistance = 100.0f;

        public override SensorResult Evaluate()
        {
            if (a == null || b == null)
            {
                Debug.LogError("DistanceSensor: One or both Transforms are not assigned.");
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
                Value = evalFunc.Evaluate(new[] { Vector3.Distance(a.position, b.position), bias }),
                Max = maxDistance,
                Min = 0.0f,
                Inputs = new List<SensorResult>()
            };
        }
    }
}