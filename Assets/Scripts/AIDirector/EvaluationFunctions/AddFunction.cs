using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace AIDirector.EvaluationFunctions
{
    [CreateAssetMenu(menuName = "SensorFunctions/Add")]
    public class AddFunction : EvaluationFunction
    {
        public override float Evaluate(float[] inputs)
        {
            if(inputs == null || inputs.Length <= 1)
            {
                Debug.LogError("AddFunction received null or not enough inputs.");
                return float.NaN; // Return 0 if no inputs are provided
            }
            //performs a left fold (or reduction) starting from 1f and applying the multiplication step-by-step:
            return inputs.Aggregate(0f, (a, b) => a + b);
        }
    }
}