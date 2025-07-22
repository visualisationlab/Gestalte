using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIDirector
{
    public class SensorController:MonoBehaviour
    {
        [ContextMenu("SimulateEvaluateAll")]
        public List<SensorResult> EvaluateAll()
        {
            // Step 1: Get all sensors in the scene
            var allSensors = FindObjectsOfType<Sensor>().ToList();

            // Step 2: Get all sensors that are inputs to composite sensors
            var referencedSensors = new HashSet<Sensor>();
            foreach (var sensor in allSensors.OfType<CompositeSensor>())
            {
                foreach (var input in sensor.Inputs)
                {
                    if (input != null)
                        referencedSensors.Add(input);
                }
            }

            // Step 3: Identify root sensors (not used as inputs)
            var rootSensors = allSensors.Except(referencedSensors).ToList();

            // Step 4: Evaluate each root sensor recursively
            var results = new List<SensorResult>();
            foreach (var root in rootSensors)
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