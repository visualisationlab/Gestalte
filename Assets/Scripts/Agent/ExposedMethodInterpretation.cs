using System;

namespace Mediator
{
    [Serializable]
    public struct ExposedMethodInterpretation
    {
        public string method;
        public string description;
        public string gameObjectName;
        public string methodGuid;
    }
}