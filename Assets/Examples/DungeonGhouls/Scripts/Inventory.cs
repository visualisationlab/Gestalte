using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<Slot> slots;
    public InputActionReference selectAction;
    public int preselectedSlot = 0;
    
    
    private void Start()
    {
        selectAction.action.performed += ctx => SelectSlot(ctx);
        HandleNumber(preselectedSlot);
    }

    public void SelectSlot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        DeselectAllSlots();
        
        var control = context.control;
        switch (control.name)
        {
            case "1": HandleNumber(0); break;
            case "2": HandleNumber(1); break;
            case "3": HandleNumber(2); break;
            case "4": HandleNumber(3); break;
            default: Debug.LogWarning("Unhandled key: " + control.name); break;
        }
    }
    
    private void HandleNumber(int number)
    {
        slots[number].Select();
    }

    private void DeselectAllSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Deselect();
        }
    }
}
