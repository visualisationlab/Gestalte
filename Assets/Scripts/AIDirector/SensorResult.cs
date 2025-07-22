using System;
using System.Collections.Generic;

namespace AIDirector
{
    public class SensorResult
    {
        public Sensor Sensor;
        public float Value;
        public float Min;
        public float Max;
        public List<SensorResult> Inputs { get; set; } = new();
    }
}