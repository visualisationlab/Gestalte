using TMPro;
using UnityEngine;

public class UIProgramMachineInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI machineName;
    [SerializeField] private TextMeshProUGUI machineDescription;
    [SerializeField] private TextMeshProUGUI machineStatus;

    public void SetMachineInfo(ExposeMachine machine)
    {
        machineName.text = machine.name;
        machineDescription.text = machine.description;
        machineStatus.text = machine.GetStatus();
    }
    
}
