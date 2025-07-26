using UnityEngine;

namespace Director.Sensors
{
    public class DistanceToCaptainSensor : DistanceSensor
    {
        private bool foundCaptain = false;
        public Vector2 directionToCaptain;

        void Update()
        {
            if (!foundCaptain)
            {
                FindCaptain();
                foundCaptain = true;
            }
        }

        private void FindCaptain()
        {
            // This method is called once to find the Captain
            other = GameObject.FindWithTag("Captain")?.transform;
            if (other == null)
            {
                Debug.LogError("DistanceToCaptainSensor: No GameObject with tag 'Captain' found.");
            }
        }

        public override SensorResult Evaluate()
        {
            if (other == null)
            {
                Debug.LogError("DistanceToCaptainSensor: other Transform is not assigned.");
                return new SensorResult
                {
                    description = description,
                    sensor = this,
                    value = float.NaN,
                    max = maxDistance,
                    min = 0.0f
                };
            }

            directionToCaptain = (Vector2)(other.position - transform.position).normalized;

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
