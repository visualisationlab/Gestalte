using System;

namespace AIDirector
{
    [Serializable]
    public class SensorFunctionMapping
    {
        public Sensor sensor;
        public CorrelatorFunction function;

        public float Evaluate()
        {
            return function.Evaluate(sensor.Evaluate());
        }

        public SensorResultDTO AsData()
        {
            return sensor.Evaluate().AsData();
        }
        
    }
}