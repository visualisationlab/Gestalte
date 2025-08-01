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
        string response = await directAPI.SendMessageAsync(systemMessage, message);
        
        string json = ExtractJson(response);
        if (!string.IsNullOrEmpty(json))
        {
            var resp = JsonConvert.DeserializeObject<RobotAgentResponse>(json);
            return resp;
        }

        return null;
    }

    private string BuildInstructions(ExposeMachine machine)
    {
        var instr = gameObjectInstructions
                  + machine.description
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
