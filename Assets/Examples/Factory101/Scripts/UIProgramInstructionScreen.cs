using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgramInstructionScreen : MonoBehaviour
{
    [SerializeField] TMP_InputField inputConsole;
    [SerializeField] Button submitButton;
    [SerializeField] MachineSelectionManager machineSelectionManager;

    public void ProcessInput()
    {
        machineSelectionManager.SendInstructions(inputConsole.text);
    }
    
}
