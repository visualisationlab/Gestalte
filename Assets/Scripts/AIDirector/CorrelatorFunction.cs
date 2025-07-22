using UnityEngine;

namespace AIDirector
{
    public abstract class CorrelatorFunction:ScriptableObject
    {
        public abstract float Evaluate(SensorResult result);
        
    }
}