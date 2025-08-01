using System;
using System.Collections.Generic;
using Examples.Factory101.Scripts;
using UnityEngine;
using UnityEngine.Events;
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
    }

    public Tool currentTool;
    public InputStateMachine inputStateMachine;
    public GameObject currentPlaceable;
    public InputController toolInputController;
    
    [Header("Placeable Prefabs")]
    public GameObject buildToolGhost;
    public GameObject conveyorBelt;
    public GameObject wallPrefab;
    public GameObject mixerPrefab;
    public GameObject furnacePrefab;

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

    public void SetToolFurnace(bool state)
    {
        SetTool(Tool.BuildFurnace, state);
        currentPlaceable = furnacePrefab;
        InstantiatePlaceable(furnacePrefab);
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
