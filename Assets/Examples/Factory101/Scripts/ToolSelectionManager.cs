using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelectionManager : MonoBehaviour
{
    [Serializable]
    public enum Tool
    {
        None,
        BuildConveyor,
        BuildWall,
        BuildMixer,
        BuildFurnace,
        BuildSprayPaint,
        BuildSplitterConveyor,
        BuildActuator,
        BuildExcavator,
        BuildFilter,
        BuildDrone,
        BuildMagnet,
        BuildSensor
    }

    public Tool currentTool;
    public InputStateMachine inputStateMachine;
    public GameObject currentPlaceable;
    public InputController toolInputController;
    // Listener so we don't have to add and remove listeners every time we change the tool
    private UnityEngine.Events.UnityAction placeListener;

    [Header("Placeable Prefabs")]
    public GameObject buildToolGhost;
    public GameObject conveyorBelt;
    public GameObject wallPrefab;
    public GameObject mixerPrefab;
    public GameObject furnacePrefab;
    public GameObject sprayPaintPrefab;
    public GameObject splitterConveyorPrefab;
    public GameObject actuatorPrefab;
    public GameObject excavatorPrefab;
    public GameObject filterPrefab;
    public GameObject dronePrefab;
    public GameObject magnetPrefab;
    public GameObject sensorPrefab;

    [SerializeField] private List<BuyableMachineButton> toolButtons;
    public void Start()
    {
        ResetTool();
        SetBuyableButtonStates();
    }
    
    public void SetToolConveyor(bool state)
    {
        SetTool(Tool.BuildConveyor, state);
        currentPlaceable = conveyorBelt;
        InstantiatePlaceable(conveyorBelt);
    }
    
    public void SetToolSplitterConveyor(bool state)
    {
        SetTool(Tool.BuildSplitterConveyor, state);
        currentPlaceable = splitterConveyorPrefab; // Assuming splitter conveyor uses the same prefab
        InstantiatePlaceable(splitterConveyorPrefab);
    }

    public void SetToolActuator(bool state)
    {
        SetTool(Tool.BuildActuator, state);
        currentPlaceable = actuatorPrefab;
        InstantiatePlaceable(actuatorPrefab);
    }

    public void SetToolWall(bool state)
    {
        SetTool(Tool.BuildWall, state);
        currentPlaceable = wallPrefab;
        InstantiatePlaceable(wallPrefab);
    }

    public void SetToolMixer(bool state)
    {
        SetTool(Tool.BuildMixer, state);
        currentPlaceable = mixerPrefab;
        InstantiatePlaceable(mixerPrefab);
    }

    public void SetToolExcavator(bool state)
    {
        SetTool(Tool.BuildExcavator, state);
        currentPlaceable = excavatorPrefab;
        InstantiatePlaceable(excavatorPrefab);
    }

    public void SetToolFilter(bool state)
    {
        SetTool(Tool.BuildFilter, state);
        currentPlaceable = filterPrefab;
        InstantiatePlaceable(filterPrefab);
    }

    public void SetToolDrone(bool state)
    {
        SetTool(Tool.BuildDrone, state);
        currentPlaceable = dronePrefab;
        InstantiatePlaceable(dronePrefab);
    }

    public void SetToolMagnet(bool state)
    {
        SetTool(Tool.BuildMagnet, state);
        currentPlaceable = magnetPrefab;
        InstantiatePlaceable(magnetPrefab);
    }

    public void SetToolSensor(bool state)
    {
        SetTool(Tool.BuildSensor, state);
        currentPlaceable = sensorPrefab;
        InstantiatePlaceable(sensorPrefab);
    }
    

    public void SetToolFurnace(bool state)
    {
        SetTool(Tool.BuildFurnace, state);
        currentPlaceable = furnacePrefab;
        InstantiatePlaceable(furnacePrefab);
    }
    
    public void SetToolSprayPaint(bool state)
    {
        SetTool(Tool.BuildSprayPaint, state);
        currentPlaceable = sprayPaintPrefab;
        InstantiatePlaceable(sprayPaintPrefab);
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

        // Reset the placelistener so we don't get double placements
        if (placeListener != null)
        {
            toolInputController.OnClickedOutside.RemoveListener(placeListener);
            placeListener = null;
        }

        var ghostDraggable = buildToolGhost.GetComponent<Draggable>();
        ghostDraggable.StopDragging();
        buildToolGhost.transform.position = Vector3.one * 99999f;
    }

    private void InstantiatePlaceable(GameObject placeablePrefab)
    {
        var ghostDraggable = buildToolGhost.GetComponent<Draggable>();
        toolInputController.ForceDraggable(ghostDraggable);
        var ghost = ghostDraggable.GetComponent<BuildToolGhost>();
        // ghost.SetPlaceablePrefab(placeablePrefab);

        if (placeListener != null)
            toolInputController.OnClickedOutside.RemoveListener(placeListener);

        // placeListener = ghost.PlaceCurrent;
        toolInputController.OnClickedOutside.AddListener(placeListener);
    }
    
    public void SetBuyableButtonStates()
    {
        foreach (var tool in toolButtons)
        {
            var toggle = tool.GetComponent<Toggle>();
            toggle.interactable = false;
            if (tool.BuyableReference.GetPrice() <= GameInfoManager.Instance.GetMoney())
            {//can buy
                toggle.interactable = true;
            }
        }
    }
}
