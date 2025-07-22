using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace AIDirector
{
    public class DirectorController:MonoBehaviour
    {
        public List<Correlator> correlators;
        public int selectionAmount;
        
        [ContextMenu("Evaluate")]
        public List<CorrelatorResult> EvaluateCorrelators()
        {
            List<CorrelatorResult> evaluated = new();
            foreach (var correlator in correlators)
            {
                evaluated.Add(new CorrelatorResult
                {
                    description = correlator.description,
                    relevance = correlator.Evaluate(),
                    sensors = correlator.sensorMappings.Select(mapping => mapping.AsData()).ToList()
                });
            }
            //order correlators by reference and return n amounts
            var ordered = evaluated.OrderBy(x => x.relevance).Take(selectionAmount).ToList();
            Debug.Log(ordered.Count);
            
            string json = JsonConvert.SerializeObject(ordered);
            Debug.Log(json);
            return ordered;
        }
        
    }
}