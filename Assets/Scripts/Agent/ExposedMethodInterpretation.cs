using System;

namespace Agent
{
    [Serializable]
    public struct ExposedMethodInterpretation
    {
        public string methodName;
        public string methodNameClean;
        public string description;
    }
}