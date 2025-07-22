using System;

namespace AIDirector
{
    [Serializable]
    public class SensorFunctionMapping
    {
        public string name;
        public Sensor sensor;
        public CorrelatorFunction function;

        public float Evaluate()
        {
            return function.Evaluate(sensor.Evaluate());
        }
    }
}