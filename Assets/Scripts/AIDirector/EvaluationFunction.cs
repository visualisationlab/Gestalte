using UnityEngine;

namespace AIDirector
{
    public abstract class EvaluationFunction : ScriptableObject
    {
        public abstract float Evaluate(float[] inputs);
    }
}