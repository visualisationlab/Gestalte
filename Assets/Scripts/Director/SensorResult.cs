using System;

namespace Director
{
    [Serializable]
    public class SensorResult
    {
        public string description;
        public Sensor sensor;
        public float value;
        public float min;
        public float max;

        public SensorResultDTO AsData()
        {
            return new SensorResultDTO
            {
                Description = description,
                Max = max,
                Min = min,
                Value = value
            };
        }
    }

    public struct SensorResultDTO
    {
        public string Description;
        public float Value;
        public float Min;
        public float Max;
    }
}