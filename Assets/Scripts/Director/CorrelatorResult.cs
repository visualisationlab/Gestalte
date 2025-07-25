using System;
using System.Collections.Generic;

namespace Director
{
    [Serializable]
    public struct CorrelatorResult
    {
        public string description;
        public float relevance;
        public List<SensorResultDTO> sensors;
    }
}