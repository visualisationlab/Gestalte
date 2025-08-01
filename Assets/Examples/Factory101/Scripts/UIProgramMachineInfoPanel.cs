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
    [SerializeField] private GameObject programmingPanel;
    
    private ExposeMachine currentMachine;
    
    public void SetMachineInfo(ExposeMachine machine)
    {
        currentMachine = machine;
        machineName.text = machine.name;
        machineDescription.text = machine.description;
        machineStatus.text = machine.GetStatus();
        
        
        if(!currentMachine.MaxUpgradeLevelReached()){
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
    }

    private void OnDisable()
    {
        currentMachine = null;
    }

    public void CallUpgrade()
    {
        if (currentMachine.CanAffordUpgrade() && !currentMachine.MaxUpgradeLevelReached())
        {
            currentMachine.Upgrade();
            KickMachineInfo();
        }
    }
    
    public void CallSale()
    {
        GameInfoManager.Instance.AddMoney(currentMachine.GetSellPrice());
        Destroy(currentMachine.gameObject);
        programmingPanel.SetActive(false);
    }
    
    //After changing something kick it to update text
    public void KickMachineInfo()
    {
        if(currentMachine) SetMachineInfo(currentMachine);
    }
    
}
