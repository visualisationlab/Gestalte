using System;
using System.Collections.Generic;
using UnityEngine;

namespace Director
{
    [Serializable]
    public class AllSensorCorrelator : Correlator
    {
        public CorrelatorFunction evaluateFunction;

        [ContextMenu("Get All Sensors in Level")]
        void getAndAddAllSensorsInLevel()
        {
            Sensor[] sensors = FindObjectsOfType<Sensor>();
            foreach (Sensor sensor in sensors)
            {
                if (sensor != null && !sensorMappings.Exists(mapping => mapping.sensor == sensor))
                {
                    sensorMappings.Add(new SensorFunctionMapping{sensor = sensor, function = evaluateFunction});
                }
            }
        }
    }
}