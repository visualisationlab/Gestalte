using System.Linq;
using UnityEngine;

namespace AIDirector.SensorOperations
{
    [CreateAssetMenu(menuName = "SensorOperations/Add")]
    public class SensorAddOperation : SensorOperation
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