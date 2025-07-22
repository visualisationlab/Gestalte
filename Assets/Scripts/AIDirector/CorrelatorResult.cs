using System;

namespace AIDirector
{
    [Serializable]
    public struct CorrelatorResult
    {
        public string description;
        public int uuid;
        public float relevance;
    }
}