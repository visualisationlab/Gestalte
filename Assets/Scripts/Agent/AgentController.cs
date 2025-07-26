using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Director;
using Mediator;
using UnityEngine;
using Newtonsoft.Json;

namespace Agent
{
    public class AgentController : MonoBehaviour
    {
        [TextArea(3, 10)] public string prePrompt = @"Explanation of the system:
You are an agent tool within a Unity system build to let LLM make decisions based on system information given.
You get references to game objects and exposed methods in the following fields: 
- allExposedGameObjects
- allExposedMethods

Within your response you can reference these game objects and methods by their GUID


Explanation on how to respond:
You respond in JSON formatted like the following:
{
""MethodGuid"": ""string"",
""Parameters"": [],
""ShortReasoning"": ""string""
}

Explanation of your characteristics:";

        [TextArea(3, 10)] public string systemDescription = "";

        [Header("References")] public ScopeController scopeController;
        public Player2Npc player2Npc;//TODO: DO Not reference player2NPC!

        private string allExposedMethods;
        private string allExposedGameObjects;
        private List<CorrelatorResult> correlatorResults = new();

        [ContextMenu("Interpret Exposed Methods")]
        public string InterpretExposedMethods()
        {
            var allObjects = FindObjectsOfType<MonoBehaviour>(true);
            var allExposedInterpretations = new List<ExposedMethodInterpretation>();

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

                        // var interpretation = new ExposedMethodInterpretation
                        // {
                        //     method = methodDescription,
                        //     description = attr.DisplayName ?? "No description provided",
                        //     methodGuid = methodGuid.ToString(),
                        //     gameObjectGuid = InstanceTracker.RetrieveGuid(obj.gameObject)
                        // };

                        // allExposedInterpretations.Add(interpretation);
                    }
                }
            }

            // Serialize the interpretations to JSON
            string json = JsonConvert.SerializeObject(allExposedInterpretations);
            Debug.Log(json);
            return json;
        }

        public void InvokeExposedMethods(string json)
        {
            ResponseStructure response = JsonConvert.DeserializeObject<ResponseStructure>(json);

            var methodInfo = MethodTracker.RetrieveMethodInfo(response.MethodGuid);

            var methodParams = methodInfo.GetParameters();
            object[] coerced = new object[response.Parameters.Length];

            for (int i = 0; i < response.Parameters.Length; i++)
            {
                var targetType = methodParams[i].ParameterType;
                coerced[i] = Convert.ChangeType(response.Parameters[i], targetType);
            }

            MethodTracker.Invoke(response.MethodGuid, coerced);
        }


        [ContextMenu("Interpret Exposed GameObjects")]
        public string InterpretExposedGameObjects()
        {
            var allObjects = FindObjectsOfType<ExposeGameObject>(true);
            var allExposedGameObjects = new List<ExposedGameObjectInterpretation>();
            Debug.Log($"Found {allObjects.Length} exposed game objects.");
            foreach (var obj in allObjects)
            {
                var gameObjectGUID = InstanceTracker.Subscribe(obj.gameObject);
                allExposedGameObjects.Add(
                    new ExposedGameObjectInterpretation()
                    {
                        gameObjectGuid = gameObjectGUID,
                        description = obj.description,
                    });
            }

            string json = JsonConvert.SerializeObject(allExposedGameObjects);
            Debug.Log(json);
            return json;
        }

        private void Start()
        {
            allExposedGameObjects = InterpretExposedGameObjects();
            allExposedMethods = InterpretExposedMethods();
            PrePrompt();
        }

        public void OnResponseReceived(NpcApiChatResponse response)
        {
            Debug.Log("Response received: " + ExtractJson(response.message));
            InvokeExposedMethods(ExtractJson(response.message));
        }
        
        private string ExtractJson(string input)
        {
            var match = Regex.Match(input, @"```json\s*(\{[\s\S]*?\})\s*```");
            return match.Success ? match.Groups[1].Value : null;
        }

        private void PrePrompt()
        {
            string agentInstructionPrompt = prePrompt + "\n" + systemDescription + "\n\nExposedMethods" +
                                            allExposedMethods + "\n\nExposedGameObjects" + allExposedGameObjects;
            
            Debug.Log($"Sending Agent Instuction Prompt: {agentInstructionPrompt}");
            player2Npc.SpawnNpcAsync(agentInstructionPrompt);
        }

        [ContextMenu("Execute Prompt")]
        public void ExecutePrompt()
        {
            var correlatorResults = scopeController.EvaluateCorrelators();
            string json = JsonConvert.SerializeObject(correlatorResults);
            string prompt = "CorrelatorResults: " + json;
            Debug.Log($"Executing prompt: {prompt}");
            player2Npc.OnChatMessageSubmitted(prompt);
        }
    }
}