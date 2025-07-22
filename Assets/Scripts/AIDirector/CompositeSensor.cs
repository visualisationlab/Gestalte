using System.Collections.Generic;
using System.Linq;

namespace AIDirector
{
    public class CompositeSensor : Sensor
    {
        public EvaluationFunction evalFunc;
        public List<Sensor> Inputs;

        public override SensorResult Evaluate()
        {
            var inputResults = Inputs.Select(i => i.Evaluate()).ToList();
            var inputValues = inputResults.Select(r => r.Value).ToArray();

            float value = evalFunc.Evaluate(inputValues);

            return new SensorResult
            {
                Sensor = this,
                Value = value,
                Inputs = inputResults
            };
        }
    } 
}
