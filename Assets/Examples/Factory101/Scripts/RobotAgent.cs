using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class RobotAgent : MonoBehaviour
{
    [TextArea(3, 50)] public string preprompt;
    [TextArea] public string gameObjectInstructions;
    
    [SerializeField] private DirectAPI directAPI;
    public async Task<RobotAgentResponse> SendMessageDirectMachine(ExposeMachine machine)
    {
        var systemMessage = preprompt + RobotAgentResponse.Format();
        var message = BuildInstructions(machine);
        Debug.Log($"Sending {message}");
        string response = await directAPI.SendMessageAsync(systemMessage, message); // may throw

        string json = ExtractJson(response);
        if (string.IsNullOrEmpty(json))
            throw new Exception("No JSON block found in assistant response.");

        var resp = JsonConvert.DeserializeObject<RobotAgentResponse>(json);
        if (resp == null)
            throw new Exception("Deserialized RobotAgentResponse was null.");

        return resp;
    }

    private string BuildInstructions(ExposeMachine machine)
    {
        var instr = gameObjectInstructions
                  + machine.GetDescription()
                  + ". It contains the following functions you can reference: "
                  + machine.GetExposedMethodsNames()
                  + ". Write code that does the following: "
                  + machine.instructionPrompt;
        return instr;
    }

    private string ExtractJson(string input)
    {
        var match = Regex.Match(input, @"```json\s*(\{[\s\S]*?\})\s*```");
        return match.Success ? match.Groups[1].Value : null;
    }
}
