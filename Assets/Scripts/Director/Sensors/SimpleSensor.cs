namespace Director.Sensors
{
    public class SimpleSensor : Sensor
    {
        public SensorProbe probe;
        public float maxValue = 100.0f;
        public float minValue = 0.0f;
        public string description;

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

