using System.Collections.Generic;
using System.Reflection;
using Mediator;
using UnityEngine;
using Newtonsoft.Json;

namespace Agent
{
    public class Agent : MonoBehaviour
    {
        [TextArea(3, 10)]
        public string prePrompt;

        public string mimicGuid;
        
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
                        var guid = InstanceTracker.Subscribe(obj); // Register the object in the InstanceTracker
                        string methodDescription = $"{method.Name}({string.Join(", ", parameterDescriptions)})";

                        var interpretation = new ExposedMethodInterpretation
                        {
                            method = methodDescription,
                            description = attr.DisplayName ?? "No description provided",
                            objectGuid = guid,
                            gameObjectName = obj.name
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

        public void InvokeExposedFunction(string json)
        {
            ResponseStructure response = JsonConvert.DeserializeObject<ResponseStructure>(json);
            object obj = InstanceTracker.Retrieve(response.ObjectGuid); // Retrieve the object using the GUID
            
            // method.Invoke(obj, null);
            
            Debug.Log(response.ObjectGuid);
        }
        
        [ContextMenu("Test Mimic")]
        public void Mimic()
        {
            string escapeJson = "{\n" +
                                $"  \"ObjectGuid\": \"{mimicGuid}\",\n" +
                                "  \"Method\": \"Dig(\\\"EscapeTunnel\\\", 5, 10)\",\n" +
                                "  \"ShortReasoning\": \"The mole is capable of digging and the floor is dirt, making this the most effective escape method.\"\n" +
                                "}";
            InvokeExposedFunction(escapeJson);
        }
    }
}