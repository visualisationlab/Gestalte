using UnityEngine;

namespace Director
{
    [CreateAssetMenu(fileName = "ExternalProbe", menuName = "Sensors/ExternalProbe")]
    public class ExternalProbe : ScriptableObject
    {
        public float value;

        public float Evaluate()
        {
            return value;
        }
    }
}