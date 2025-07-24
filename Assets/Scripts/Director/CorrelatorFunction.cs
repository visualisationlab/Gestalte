using UnityEngine;

namespace Director
{
    public abstract class CorrelatorFunction:ScriptableObject
    {
        public abstract float Evaluate(SensorResult result);
        
    }
}