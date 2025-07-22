using System.Linq;
using UnityEngine;

namespace AIDirector.EvaluationFunctions
{
    [CreateAssetMenu(menuName = "SensorFunctions/Add")]
    public class AddFunction : EvaluationFunction
    {
        public override float Evaluate(float[] inputs)
        {
            //performs a left fold (or reduction) starting from 1f and applying the multiplication step-by-step:
            return inputs.Aggregate(0f, (a, b) => a + b);
        }
    }
}