using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIProgramInstructionScreen : MonoBehaviour
{
    [SerializeField] TMP_InputField inputConsole;
    [SerializeField] private GameObject rootPanel;
    [SerializeField] GameObject processingScreen;
    [SerializeField] TextMeshProUGUI processingErrorMessage;
    [SerializeField] UIMethodInstructionItem methodInstructionTemplate;
    [SerializeField] Transform methodInstructionView;
    [SerializeField] UIOnClick freezeClick;

    [Header("Info Panel")]
    [SerializeField] private TextMeshProUGUI machineName;

    [SerializeField] private TextMeshProUGUI machineDescription;
    [SerializeField] private TextMeshProUGUI machineStatus;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradePrice;
    [SerializeField] private TextMeshProUGUI sellPrice;
    // [SerializeField] private GameObject programmingPanel;

    [Header("Programming")]
    [SerializeField] private RobotAgent agent;
    
    [Header("Events")]
    private List<UIMethodInstructionItem> placedMethodInstructions = new();

    private ExposeMachine selectedMachine;
    
    public async void ProcessInput()
    {
        try
        {
            processingScreen.SetActive(true);
            processingErrorMessage.text = "";
            freezeClick.enabled = false;
            selectedMachine.instructionPrompt = inputConsole.text;
            var response = await agent.SendMessageDirectMachine(selectedMachine); // exceptions bubble
            selectedMachine.SetScript(response.Lua);

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
            SetMachine(selectedMachine);
        }
    }

    public void SetMachine(ExposeMachine machine)
    {
        selectedMachine = machine;
        inputConsole.text = machine.instructionPrompt;
        machineName.text = machine.name;
        machineDescription.text = machine.GetDescription();
        machineStatus.text = machine.GetStatus();
        
        if(!selectedMachine.MaxUpgradeLevelReached()){
            upgradePrice.text = $"-{machine.GetUpgradePrice().ToString()}";
            upgradePrice.color = machine.CanAffordUpgrade() ? Color.yellow : Color.red;
            upgradeButton.interactable = machine.CanAffordUpgrade();
        }
        else
        {
            upgradePrice.color = Color.grey;
            upgradePrice.text = "Max Upgraded";
            upgradeButton.interactable = false;
        }
        
        sellPrice.text = $"+{machine.GetSellPrice().ToString()}";
        SetMethodInstructions(machine.GetExposedMethods());
    }
    public void SetMethodInstructions(List<ExposedMethodInterpretation> methodInterpretations)
    {
        ClearMethodInstructions();
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
    
    public void CallUpgrade()
    {
        if (selectedMachine.CanAffordUpgrade() && !selectedMachine.MaxUpgradeLevelReached())
        {
            selectedMachine.Upgrade();
            SetMachine(selectedMachine);
        }
    }
    
    public void CallSale()
    {
        GameInfoManager.Instance.AddMoney(selectedMachine.GetSellPrice());
        Destroy(selectedMachine.gameObject);
        rootPanel.SetActive(false);
    }
}
