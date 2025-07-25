using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<Slot> slots;
    public InputActionReference selectAction;
    public int selectedSlot = 0;
    public UnityEvent<int> OnSwitch;
    
    private void Start()
    {
        selectAction.action.performed += ctx => SelectSlot(ctx);
        HandleNumber(selectedSlot);
    }

    public void SelectSlot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        DeselectAllSlots();
        selectedSlot = int.Parse(context.control.name) - 1;
        HandleNumber(selectedSlot);
    }
    
    private void HandleNumber(int number)
    {
        OnSwitch?.Invoke(number);
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
