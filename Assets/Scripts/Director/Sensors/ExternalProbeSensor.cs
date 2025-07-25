using UnityEngine;

namespace Director.Sensors
{
    public class ExternalProbeSensor:Sensor
    {
        public ExternalProbe probe;
        
        [ContextMenu("Force Probe")]
        public override SensorResult Evaluate()
        {
            return new SensorResult
            {
                description = description,
                sensor = this,
                value = probe.Evaluate(),
                max = maxValue,
                min = minValue
            };
        }
    }
}