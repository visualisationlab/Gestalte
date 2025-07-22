using UnityEngine;

namespace AIDirector
{
    public abstract class Sensor:MonoBehaviour
    {
        public abstract SensorResult Evaluate();
    }
}