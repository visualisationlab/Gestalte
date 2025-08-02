using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelectionManager : MonoBehaviour
{
    private List<BuyableMachineButton> toolButtons = new();

    private void Start()
    {
        toolButtons = GetComponentsInChildren<BuyableMachineButton>().ToList();
        SetBuyableButtonStates();
    }

    public void SetBuyableButtonStates()
    {
        foreach (var tool in toolButtons)
        {
            var toggle = tool.GetComponent<Toggle>();
            toggle.interactable = false;
            
            if (tool.isFree)
            {
                toggle.interactable = true;
                continue;
            }
            
            if (tool.BuyableReference.GetPrice() <= GameInfoManager.Instance.GetMoney())
            {
                toggle.interactable = true;
            }
        }
    }
}
