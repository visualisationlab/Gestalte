using TMPro;
using UnityEngine;

public class UIProgramMachineInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI machineName;
    [SerializeField] private TextMeshProUGUI machineDescription;
    [SerializeField] private TextMeshProUGUI machineStatus;

    private ExposeMachine currentMachine;
    
    public void SetMachineInfo(ExposeMachine machine)
    {
        currentMachine = machine;
        machineName.text = machine.name;
        machineDescription.text = machine.description;
        machineStatus.text = machine.GetStatus();
    }
    
    
    //After changing something kick it to update text
    public void KickMachineInfo()
    {
        SetMachineInfo(currentMachine);
    }
    
}
