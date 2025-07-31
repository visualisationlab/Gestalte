using System;
using Unity.VisualScripting;
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
    public GameObject currentPlaceable;
    public InputController toolInputController;
    
    [Header("Placeable Prefabs")]
    public GameObject buildToolGhost;
    public GameObject conveyorBelt;

    public void Start()
    {
        ResetTool();
    }

    public void SetToolConveyor(bool state)
    {
        SetTool(Tool.BuildConveyor, state);
        currentPlaceable = conveyorBelt;
        InstantiatePlaceable(conveyorBelt);
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
        currentPlaceable = null;
        var ghostDraggable = buildToolGhost.GetComponent<Draggable>();
        ghostDraggable.StopDragging();
        buildToolGhost.transform.position = Vector3.one * 99999f;
    }

    private void InstantiatePlaceable(GameObject placeablePrefab)
    {
        var ghostDraggable = buildToolGhost.GetComponent<Draggable>();
        toolInputController.ForceDraggable(ghostDraggable);
        var ghost = ghostDraggable.GetComponent<BuildToolGhost>();
        ghost.SetPlaceablePrefab(placeablePrefab);
        toolInputController.OnClickedOutside.AddListener(ghost.PlaceCurrent);
    }
}
