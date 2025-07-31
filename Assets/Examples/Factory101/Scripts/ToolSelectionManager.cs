using System;
using UnityEngine;

public class ToolSelectionManager : MonoBehaviour
{
    [Serializable]
    public enum Tool
    {
        None,
        BuildConveyor
    }

    public Tool currentTool;
    public InputStateMachine inputStateMachine;

    public void SetToolConveyor(bool state)
    {
        SetTool(Tool.BuildConveyor, state);
    }
    
    private void SetTool(Tool tool, bool state)
    {
        if (!state)
        {
            ResetTool();
        }
        else
        {
            inputStateMachine.SetBuildState();
            currentTool = tool;
        }
    }

    public void ResetTool()
    {
        inputStateMachine.SetInteractState();
        currentTool = Tool.None;
    }
}
