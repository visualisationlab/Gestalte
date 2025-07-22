using System.Collections.Generic;
using UnityEngine;

namespace AIDirector
{
    public abstract class CorrelatorOperation : ScriptableObject
    {
        public abstract float Evaluate(List<SensorResult> inputs);
    }
}