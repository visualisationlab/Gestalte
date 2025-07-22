using System.Collections.Generic;
using UnityEngine;

namespace AIDirector.Sensors
{
    public class DistanceSensor : Sensor
    {
        public EvaluationFunction evalFunc;
        public Transform a;
        public Transform b;
        public float bias = 1.0f;

        public override SensorResult Evaluate()
        {
            if (a == null)
                throw new System.NullReferenceException("DistanceSensor: Transform a is not assigned.");
            if (b == null)
                throw new System.NullReferenceException("DistanceSensor: Transform b is not assigned.");

            return new SensorResult
            {
                Sensor = this, 
                Value = evalFunc.Evaluate(new[]{Vector3.Distance(a.position, b.position), bias}),
                Inputs = new List<SensorResult>()
            };
        }
    }
}