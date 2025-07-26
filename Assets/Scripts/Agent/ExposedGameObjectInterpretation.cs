using System;
using System.Collections.Generic;

namespace Agent
{
    [Serializable]
    public struct ExposedGameObjectInterpretation
    {
        public string gameObjectGuid;
        public string description;
        public List<ExposedMethodInterpretation> methods;
    }
}