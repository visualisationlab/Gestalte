using System.Collections.Generic;
using System.Linq;

namespace AIDirector
{
    public class CompositeSensor : Sensor
    {
        public SensorOperation evalFunc;
        public List<Sensor> Inputs;

        public override SensorResult Evaluate()
        {
            var inputResults = Inputs.Select(i => i.Evaluate()).ToList();
            var inputValues = inputResults.Select(r => r.Value).ToArray();
            var inputMins = inputResults.Select(r => r.Min).ToArray();
            var inputMaxs = inputResults.Select(r => r.Max).ToArray();

            float value = evalFunc.Evaluate(inputValues);
            float min = evalFunc.Evaluate(inputMins);
            float max = evalFunc.Evaluate(inputMaxs);

            return new SensorResult
            {
                Sensor = this,
                Value = value,
                Inputs = inputResults,
                Min = min,
                Max = max
            };
        }
    } 
}
