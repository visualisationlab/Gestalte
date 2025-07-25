﻿using UnityEngine;

namespace Director.CorrelatorFunctions
{
    [CreateAssetMenu(fileName = "CorrelatorCurveFunction", menuName = "CorrelatorFunction/CurveFunction")]
    public class CorrelatorCurveFunction: CorrelatorFunction
    {
        public AnimationCurve curve;
        public override float Evaluate(SensorResult result)
        {
            if (curve == null)
            {
                Debug.LogError("CorrelatorCurveFunction: Curve is not assigned.");
                return 0.0f; // Return a default value or handle the error as needed
            }

            float normalizedValue = result.sensor.MinMaxNormalize();
            return curve.Evaluate(normalizedValue);
        }
    }
}