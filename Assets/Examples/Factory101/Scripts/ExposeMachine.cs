using System.Collections.Generic;
using System.Reflection;
using Agent;
using Mediator;
using UnityEngine;

public class ExposeMachine : MonoBehaviour
{
    [TextArea] public string description;
    public List<ExposedMethodInterpretation> GetExposedMethods()
    {
        var allObjects = gameObject.GetComponents<MonoBehaviour>();
        var resultExposedMethods = new List<ExposedMethodInterpretation>();
        foreach (var obj in allObjects)
        {
            var type = obj.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ExposeMethodAttribute>();
                if (attr != null)
                {
                    List<string> parameterDescriptions = new List<string>();

                    foreach (var param in method.GetParameters())
                    {
                        parameterDescriptions.Add($"{param.ParameterType} {param.Name}");
                    }

                    string methodDescription = $"{method.Name}({string.Join(", ", parameterDescriptions)})";
                    var methodGuid = MethodTracker.Subscribe(method, obj);

                    var interpretation = new ExposedMethodInterpretation
                    {
                        method = methodDescription,
                        description = attr.DisplayName ?? "No description provided",
                        methodGuid = methodGuid.ToString(),
                        gameObjectGuid = InstanceTracker.RetrieveGuid(obj.gameObject)
                    };
                    resultExposedMethods.Add(interpretation);
                }
            }
        }

        return resultExposedMethods;
    }
    
}