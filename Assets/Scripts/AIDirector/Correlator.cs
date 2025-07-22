using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIDirector
{
    public class Correlator : MonoBehaviour
    {
        public List<SensorFunctionMapping> sensorMappings;
        public string context;

        [ContextMenu("Test Evaluate")]
        public float Evaluate()
        {
            float total = 0.0f;
            foreach (var map in sensorMappings)
            {
                try
                {
                    var result = map.Evaluate();
                    total += result;
                    Debug.Log($"Evaluated sensor: {map.sensor.name} => {result}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error evaluating sensor {map.sensor.name}: {ex.Message}");
                }
            }
            float average = total / sensorMappings.Count;
            Debug.Log($"Average Importance: {average}");
            return average;
        }
    }
}