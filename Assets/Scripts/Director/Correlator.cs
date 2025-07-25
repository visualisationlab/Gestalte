﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Director
{
    [Serializable]
    public class Correlator : MonoBehaviour
    {
        public List<SensorFunctionMapping> sensorMappings;
        public List<Sensor> ignoredSensors; // Sensors passed along as information but not taken into account for relevance score
        [SerializeField] public string description;
        
        public float Evaluate()
        {
            float total = 0.0f;
            foreach (var map in sensorMappings)
            {
                try
                {
                    var result = map.Evaluate();
                    total += result;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error evaluating sensor {map.sensor.name}: {ex.Message}");
                }
            }
            float average = total / sensorMappings.Count;
            return average;
        }
    }
}