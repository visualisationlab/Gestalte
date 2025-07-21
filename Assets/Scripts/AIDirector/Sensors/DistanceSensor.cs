using UnityEngine;

namespace AIDirector.Sensors
{
    public class DistanceSensor : Sensor
    {
        public Transform a;
        public Transform b;
        public float bias;

        public override float Evaluate()
        {
            if (a == null)
                throw new System.NullReferenceException("DistanceSensor: Transform a is not assigned.");
            if (b == null)
                throw new System.NullReferenceException("DistanceSensor: Transform b is not assigned.");

            return Vector3.Distance(a.position, b.position) * bias;
        }
    }
}