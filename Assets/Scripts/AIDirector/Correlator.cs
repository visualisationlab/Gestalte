using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIDirector
{
    public class Correlator : MonoBehaviour
    {
        public List<Sensor> sensors;
        public string context;
        public CorrelatorOperation correlatorFunc;
        
        [ContextMenu("Test Evaluate")]
        public List<SensorResult> Evaluate()
        {
            var results = new List<SensorResult>();
            foreach (var root in sensors)
            {
                try
                {
                    var result = root.Evaluate();
                    results.Add(result);
                    Debug.Log($"Evaluated root sensor: {root.name} => {result.Value}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error evaluating sensor {root.name}: {ex.Message}");
                }
            }
            
            return results;
        }
    }
}