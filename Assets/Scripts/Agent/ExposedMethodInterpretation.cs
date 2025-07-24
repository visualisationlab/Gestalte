using System;

namespace Agent
{
    [Serializable]
    public struct ExposedMethodInterpretation
    {
        public string method;
        public string description;
        public string gameObjectGuid;
        public string methodGuid;
    }
}