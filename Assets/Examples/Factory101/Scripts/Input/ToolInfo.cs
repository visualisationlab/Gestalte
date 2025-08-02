using UnityEngine;

public class ToolInfo : MonoBehaviour
{
    public ToolManager.Tool tool;
    public ToolManager toolManager;
    [Header("Only On Placeable")]
    public GameObject placeable;
    public GameObject ghost;

    public void SelectTool()
    {
        if(tool == ToolManager.Tool.Place){
            toolManager.SelectPlaceableTool(ghost, placeable);
        }
    }
    
}
