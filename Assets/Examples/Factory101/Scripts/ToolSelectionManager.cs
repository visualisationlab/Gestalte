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

    public void SetToolConveyor(bool state)
    {
        SetTool(Tool.BuildConveyor, state);
    }
    
    private void SetTool(Tool tool, bool state)
    {
        if (!state) ResetTool(); else currentTool = tool;
    }

    public void ResetTool()
    {
        currentTool = Tool.None;
    }
}
