using System.Collections.Generic;
using System.Reflection;
using Agent;
using Examples.Factory101.Scripts;
using Mediator;
using UnityEngine;

public class ExposeMachine : MonoBehaviour
{
    [TextArea] public string description;
    [TextArea] public string instructionPrompt;
    [TextArea] public string script;
    [SerializeField] private Machine machine;
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

                    string methodFormat = $"{method.Name}({string.Join(", ", parameterDescriptions)})";

                    var interpretation = new ExposedMethodInterpretation
                    {
                        methodName = methodFormat,
                        description = attr.DisplayName ?? "No description provided"
                    };
                    resultExposedMethods.Add(interpretation);
                }
            }
        }
        return resultExposedMethods;
    }

    public string GetExposedMethodsNames()
    {
        var meth = GetExposedMethods();
        string result = "";
        foreach (var m in meth)
        {
            result += $"{m.methodName} \n";
        }

        return result;
    }

    public void SetScript(string code)
    {
        script = code;
        machine.SetScript(script);
    }
    
    
}