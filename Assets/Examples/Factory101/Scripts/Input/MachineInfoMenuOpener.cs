using Examples.Factory101.Scripts.Input;
using UnityEngine;

public class MachineInfoMenuOpener : MonoBehaviour, IClickable, IHoverable
{
    public void OnClicked()
    {
        var exposed = gameObject.GetComponent<ExposeMachine>();
        if (exposed == null) return;
        GlobalMenuManager.Instance.OpenMachineInfoScreen(exposed);
    }

    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        
    }
}