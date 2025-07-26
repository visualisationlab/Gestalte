using UnityEngine;
using UnityEngine.Events;

public class MachineSelectionManager : MonoBehaviour
{
    public UnityEvent<string> OnMachineSelectedDescription;
    public UnityEvent OnMachineDeSelected;
    public UnityEvent<string> OnMachineHoverDescription;
    public UnityEvent OnMachineHoverOut;
    [Header("Instructions")]
    public UnityEvent<string> OnMachineSelectedInstruction;
    public UnityEvent<string> OnMachineHoverInstruction;

    public ExposeMachine selectedMachine;
    
    public void HoverGameObject(GameObject go)
    {
        var machine = go.GetComponent<ExposeMachine>();
        if (machine == null) return;
        OnMachineHoverDescription.Invoke(machine.GetFullDescription());
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
        OnMachineSelectedDescription.Invoke(machine.GetFullDescription());
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
}
