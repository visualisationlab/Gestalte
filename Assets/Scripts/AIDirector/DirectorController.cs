using System.Collections.Generic;
using System.Linq;
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
                evaluated.Add(new CorrelatorResult(){relevance = correlator.Evaluate(), correlator = correlator});
            }

            var ordered = evaluated.OrderBy(x => x.relevance).Take(selectionAmount).ToList();
            Debug.Log(ordered.Count);
            return ordered;
        }
        
    }
}