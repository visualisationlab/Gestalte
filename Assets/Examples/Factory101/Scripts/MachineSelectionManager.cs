using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class MachineSelectionManager : MonoBehaviour
{
    public UnityEvent<ExposeMachine> OnMachineSelectedDescription;
    public UnityEvent OnMachineDeSelected;
    public UnityEvent<string> OnMachineHoverDescription;
    public UnityEvent OnMachineHoverOut;
    [Header("Instructions")]
    public UnityEvent<string> OnMachineSelectedInstruction;
    public UnityEvent<string> OnMachineHoverInstruction;

    public ExposeMachine selectedMachine;
    public RobotAgent agent;
    
    public void HoverGameObject(GameObject go)
    {
        var machine = go.GetComponent<ExposeMachine>();
        if (machine == null) return;
        OnMachineHoverDescription.Invoke(machine.description);
        OnMachineHoverInstruction.Invoke(machine.instructionPrompt);
    }
    
    public void HoverOutGameObject()
    {
        OnMachineHoverOut.Invoke();
    }

    public void SelectGameObject(GameObject go)
    {
        var machine = go.GetComponent<ExposeMachine>();
        if (machine == null) return;
        selectedMachine = machine;
        OnMachineSelectedDescription.Invoke(machine);
        OnMachineSelectedInstruction.Invoke(machine.instructionPrompt);
    }

    public void DeselectGameObject()
    {
        selectedMachine = null;
        OnMachineDeSelected.Invoke();
    }

    public void UpdateInstructionsForSelectedMachine(string text)
    {
        if(selectedMachine){
            selectedMachine.instructionPrompt = text;
        }
    }

    public async Task SendInstructions(string message)
    {
        if (selectedMachine == null)
        {
            Debug.LogError("No machine selected.");
            return;
        }

        selectedMachine.instructionPrompt = message;

        try
        {
            var response = await agent.SendMessageDirectMachine(selectedMachine);
            selectedMachine.SetScript(response.Lua);
            Debug.Log($"Programming RESPONSE {response}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send instructions: {e.Message}");
        }
    }
}
