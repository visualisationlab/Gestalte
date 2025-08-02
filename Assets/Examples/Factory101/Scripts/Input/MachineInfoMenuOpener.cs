using Examples.Factory101.Scripts.Input;
using UnityEngine;

public class MachineInfoMenuOpener : MonoBehaviour, IClickable, IHoverable
{
    public void OnClicked()
    {
        if (InteractionModeController.Instance.CurrentMode != InteractionMode.Default) return;
        var exposed = gameObject.GetComponent<ExposeMachine>();
        if (exposed == null) return;
        GlobalMenuManager.Instance.OpenMachineInfoScreen(exposed);
    }

    public void OnHoverEnter()
    {
        if (InteractionModeController.Instance.CurrentMode != InteractionMode.Default) return;
        var exposed = gameObject.GetComponent<ExposeMachine>();
        if (exposed == null) return;
        GlobalMenuManager.Instance.SetSideInfoPanelText(exposed.GetDescription());
    }

    public void OnHoverExit()
    {
        GlobalMenuManager.Instance.SetSideInfoPanelText("");
    }
}