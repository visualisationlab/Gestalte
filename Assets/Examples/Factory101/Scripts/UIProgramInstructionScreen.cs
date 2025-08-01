using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agent;
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
    [SerializeField] UIMethodInstructionItem methodInstructionTemplate;
    [SerializeField] Transform methodInstructionView;
    [SerializeField] UIOnClick freezeClick;

    private List<UIMethodInstructionItem> placedMethodInstructions = new();
    
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

    public void SetMethodInstructions(List<ExposedMethodInterpretation> methodInterpretations)
    {
        placedMethodInstructions.Clear();
        foreach (var item in methodInterpretations)
        {
            var methodInstruction = Instantiate(methodInstructionTemplate, methodInstructionView);
            methodInstruction.SetInstructions(item);
            placedMethodInstructions.Add(methodInstruction);
        }
    }

    public void ClearMethodInstructions()
    {
        foreach (var instruction in placedMethodInstructions)
        {
            Destroy(instruction.gameObject);
        }
        placedMethodInstructions.Clear();
    }
}
