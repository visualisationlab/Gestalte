namespace Director.Sensors
{
    public class SimpleSensor : Sensor
    {
        public SensorProbe probe;
        public float maxValue = 100.0f; //todo can be removed
        public float minValue = 0.0f; //todo can be removed
        public string description; //todo can be removed

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

