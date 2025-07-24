using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Director
{
    public class ScopeController:MonoBehaviour
    {
        public List<Correlator> correlators;
        public int selectionAmount;
        
        [ContextMenu("Evaluate")]
        public List<CorrelatorResult> EvaluateCorrelators()
        {
            List<CorrelatorResult> evaluated = new();
            foreach (var correlator in correlators)
            {
                List<SensorResultDTO> sensorResults = new List<SensorResultDTO>();
                foreach (var ignoredSensor in correlator.ignoredSensors)
                {
                    sensorResults.Add(ignoredSensor.Evaluate().AsData());
                }
                
                var sensors = correlator.sensorMappings.Select(mapping => mapping.AsData()).ToList();
                
                // merge sensorResults with sensors
                sensors.AddRange(sensorResults);
                
                evaluated.Add(new CorrelatorResult
                {
                    description = correlator.description,
                    relevance = correlator.Evaluate(),
                    sensors = sensors
                });
            }
            //order correlators by reference and return n amounts
            var ordered = evaluated.OrderBy(x => x.relevance).Take(selectionAmount).ToList();
            string json = JsonConvert.SerializeObject(ordered);
            Debug.Log(json);
            return ordered;
        }
        
    }
}