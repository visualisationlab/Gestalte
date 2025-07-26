using System.Collections.Generic;
using System.Text.RegularExpressions;
using Agent;
using Newtonsoft.Json;
using UnityEngine;

public class RobotAgent : MonoBehaviour
{
    [TextArea(3, 50)] public string preprompt;
    [TextArea] public string gameObjectInstructions;
    public Player2Npc player2Npc;
    public GameObject selectedMachine;
    private void Start()
    {
        // var interpretedGameObjects = InterpretExposedGameObjects();
        var composedPreprompt = preprompt + RobotAgentResponse.Format();
        _ = player2Npc.SpawnNpcAsync(composedPreprompt);
    }

    [ContextMenu("Send Message")]
    public void SendMessage()
    {
        // var machine = selectedMachine.GetComponent<ActuatorMachine>();
        var exposed = selectedMachine.GetComponent<ExposeMachine>();
        var instructions = gameObjectInstructions + exposed.description + ". It contains the following functions you can reference:" + exposed.GetExposedMethodsNames();
        Debug.Log(instructions);
        player2Npc.OnChatMessageSubmitted(instructions);
    }
    
    public void OnResponseReceived(NpcApiChatResponse response)
    {
        string json = ExtractJson(response.message);
        RobotAgentResponse resp = JsonConvert.DeserializeObject<RobotAgentResponse>(json);
        Debug.Log(resp.Lua);
        // var go = InstanceTracker.Retrieve(resp.GameObjectGUID);
        selectedMachine.GetComponent<ExposeMachine>().SetScript(resp.Lua);
    }
    
    private string ExtractJson(string input)
    {
        var match = Regex.Match(input, @"```json\s*(\{[\s\S]*?\})\s*```");
        return match.Success ? match.Groups[1].Value : null;
    }
    
    public static string ExtractPureLua(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Match the Lua code block with optional newline after ```lua
        var match = Regex.Match(input, @"```lua\s*\n?(.*?)\n?```", RegexOptions.Singleline);

        if (match.Success && match.Groups.Count > 1)
        {
            return match.Groups[1].Value.Trim();
        }

        return string.Empty;
    }
    
    public string InterpretExposedGameObjects()
    {
        var allObjects = FindObjectsOfType<ExposeMachine>(true);
        var allExposedGameObjects = new List<ExposedGameObjectInterpretation>();
        foreach (var obj in allObjects)
        {
            var gameObjectGUID = InstanceTracker.Subscribe(obj.gameObject);
            allExposedGameObjects.Add(
                new ExposedGameObjectInterpretation()
                {
                    gameObjectGuid = gameObjectGUID,
                    description = obj.description,
                    methods = obj.GetExposedMethods()
                });
        }

        string json = JsonConvert.SerializeObject(allExposedGameObjects);
        Debug.Log(json);
        return json;
    }

    public void SelectMachine(GameObject go)
    {
        var machineDescription = go.GetComponent<ExposeMachine>();
        if (machineDescription == null) return;
        selectedMachine = go;
        Debug.Log(selectedMachine);
    }
}
