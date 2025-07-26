using UnityEngine;
using GraphTool.Runtime;

namespace Director.Sensors
{
    public class DirectionSensor : Sensor
    {
        public Transform source;
        public Transform target;
        public float angle;
        void Start()
        {
            // Update the angle value in the GraphSubscription, the value is clamped between 0 - 1 so map to 0 - 360 degrees
            GraphSubscription.Subscribe(
                this,
                () => angle / 360f,
                Color.cyan
            );
        }

        void OnDestroy()
        {
            GraphSubscription.Unsubscribe(this);
        }
        public override SensorResult Evaluate()
        {
            if (source == null || target == null)
            {
                return new SensorResult
                {
                    value = 0f,
                    min = 0f,
                    max = 360f
                };
            }

            Vector2 direction = target.position - source.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle < 0)
                angle += 360f;

            return new SensorResult
            {
                value = angle,
                min = 0f,
                max = 360f
            };
        }
    }
}