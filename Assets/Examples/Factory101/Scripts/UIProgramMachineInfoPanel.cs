using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgramMachineInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI machineName;
    [SerializeField] private TextMeshProUGUI machineDescription;
    [SerializeField] private TextMeshProUGUI machineStatus;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradePrice;
    [SerializeField] private TextMeshProUGUI sellPrice;
    
    private ExposeMachine currentMachine;
    
    public void SetMachineInfo(ExposeMachine machine)
    {
        currentMachine = machine;
        machineName.text = machine.name;
        machineDescription.text = machine.description;
        machineStatus.text = machine.GetStatus();
        
        upgradePrice.text = machine.GetUpgradePrice().ToString();
        upgradePrice.color = machine.CanAffordUpgrade() ? Color.yellow : Color.red;
        upgradeButton.interactable = machine.CanAffordUpgrade();
        
        sellPrice.text = machine.GetSellPrice().ToString();
    }

    private void OnDisable()
    {
        currentMachine = null;
    }

    public void CallUpgrade()
    {
        if (currentMachine.CanAffordUpgrade())
        {
            
        }
    }
    
    public void CallSale()
    {
        
    }
    
    //After changing something kick it to update text
    public void KickMachineInfo()
    {
        if(currentMachine) SetMachineInfo(currentMachine);
    }
    
}
