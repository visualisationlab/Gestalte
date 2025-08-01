using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgramInstructionScreen : MonoBehaviour
{
    [SerializeField] TMP_InputField inputConsole;
    [SerializeField] Button submitButton;
    [SerializeField] MachineSelectionManager machineSelectionManager;
    [SerializeField] GameObject processingScreen;
    [SerializeField] TextMeshProUGUI processingErrorMessage;
    [SerializeField] UIOnClick freezeClick;

    public async void ProcessInput()
    {
        try
        {
            processingScreen.SetActive(true);
            processingErrorMessage.text = "";
            freezeClick.enabled = false;
            await machineSelectionManager.SendInstructions(inputConsole.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"SendInstructions failed: {e.Message}");
            processingErrorMessage.text = e.Message;
            await Task.Delay(5000);
        }
        finally
        {
            processingScreen.SetActive(false);
            freezeClick.enabled = true;
        }
    }
    
    
    public void SetInstructions(string instructions)
    {
        inputConsole.text = instructions;
    }
    
}
