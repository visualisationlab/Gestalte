using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIDirector
{
    public class SensorController:MonoBehaviour
    {
        public List<SensorResult> EvaluateAll()
        {
            var sensors = FindObjectsOfType<Sensor>().ToList();
            var results = new List<SensorResult>();
            foreach (var sensor in sensors)
            {
                try
                {
                    float value = sensor.Evaluate();
                    results.Add(new SensorResult {sensor = sensor, value = value});
                    Debug.Log($"[{sensor.GetType().Name}] => {value}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Sensor error in {sensor.name}: {e.Message}");
                }
            }
            return results;
        }
    }
}