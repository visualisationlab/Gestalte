using TMPro;
using UnityEngine;

public class UIProgramMachineInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI machineName;
    [SerializeField] private TextMeshProUGUI machineDescription;

    public void SetMachineInfo(string name, string description)
    {
        machineName.text = name;
        machineDescription.text = description;
    }
    
}
