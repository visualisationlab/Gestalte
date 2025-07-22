using System.Collections.Generic;
using UnityEngine;

namespace AIDirector.CorrelatorOperations
{
    [CreateAssetMenu(menuName = "CorrelatorOperations/Linear")]
    public class LinearOperation : CorrelatorOperation
    {
        public override float Evaluate(List<SensorResult> inputs)
        {
            return 0.0f;
        }
    }
}