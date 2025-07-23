using System.Collections.Generic;
using System.Reflection;
using Mediator;
using UnityEngine;
using Newtonsoft.Json;

namespace Agent
{
    public class Agent : MonoBehaviour
    {
        [ContextMenu("Interpret Exposed Functions")]
        private string InterpretExposedFunctions()
        {
            var allObjects = FindObjectsOfType<MonoBehaviour>(true);
            var allExposedInterpretations = new List<ExposedMethodInterpretation>();

            foreach (var obj in allObjects)
            {
                var type = obj.GetType();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var method in methods)
                {
                    var attr = method.GetCustomAttribute<ExposeFunctionAttribute>();
                    if (attr != null)
                    {
                        List<string> parameterDescriptions = new List<string>();

                        foreach (var param in method.GetParameters())
                        {
                            parameterDescriptions.Add($"{param.ParameterType} {param.Name}");
                        }

                        string methodDescription = $"{method.Name}({string.Join(", ", parameterDescriptions)})";

                        var interpretation = new ExposedMethodInterpretation
                        {
                            method = methodDescription,
                            description = attr.DisplayName ?? "No description provided"
                        };
                        
                        allExposedInterpretations.Add(interpretation);

                        // Invoke the method
                        // method.Invoke(obj, null);
                    }
                }
            }

            // Serialize the interpretations to JSON
            string json = JsonConvert.SerializeObject(allExposedInterpretations);
            Debug.Log(json);
            return json;
        }
    }
}