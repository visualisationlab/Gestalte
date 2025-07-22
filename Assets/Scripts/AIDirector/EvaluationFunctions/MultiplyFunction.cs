using System.Linq;
using UnityEngine;

namespace AIDirector.EvaluationFunctions
{
    [CreateAssetMenu(menuName = "SensorFunctions/Multiply")]
    public class MultiplyFunction : EvaluationFunction
    {
        public override float Evaluate(float[] inputs)
        {
            if(inputs == null || inputs.Length <= 1)
            {
                Debug.LogError("MultiplyFunction received null or not enough inputs.");
                return float.NaN;
            }
            //performs a left fold (or reduction) starting from 1f and applying the multiplication step-by-step:
            return inputs.Aggregate(1f, (a, b) => a * b);
        }
    }
}