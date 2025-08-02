using System;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    [Serializable]
    public enum Tool
    {
        None,
        Place
    }
    [SerializeField] public CursorController cursor;
    [SerializeField] public PointerRaycaster pointerRaycaster;
    private Tool currentTool;
    private GameObject currentPlaceablePrefab;
    private float rotation;
    private float rotationAmount = 90f;
    
    public void SelectPlaceableTool(GameObject ghost, GameObject placeablePrefab, bool canRotate)
    {
        rotation = 0;
        currentTool = Tool.Place;
        currentPlaceablePrefab = placeablePrefab;
        CursorController.Instance.ShowBuildGhost(ghost, canRotate);
    }

    public void HidePlaceableTool()
    {
        currentTool = Tool.None;
        currentPlaceablePrefab = null;
        CursorController.Instance.HideBuildGhost();
    }
    
    public void OnClick()
    {
        if (UIUtils.IsPointerOverUI()) 
            return;
        if (InteractionModeController.Instance.CurrentMode != InteractionMode.Placement)
            return;
        if (pointerRaycaster.HoverOverBlockingMachine()) 
            return;
        
        if (currentTool == Tool.Place)
        {
            PlacePlaceable();
        }
    }

    public void PlacePlaceable()
    {
        var buyable = currentPlaceablePrefab.GetComponent<IBuyable>();
        if (buyable.GetPrice() <= GameInfoManager.Instance.GetMoney())
        {
            var realPlaceable = Instantiate(currentPlaceablePrefab, CursorController.Instance.GetPositionInGrid(), Quaternion.identity);
            realPlaceable.transform.Rotate(Vector3.forward, rotation);
            AudioManager.Instance.Pluck();
            GameInfoManager.Instance.AddMoney(-buyable.GetPrice());
        }
    }

    public void Rotate()
    {
        rotation += rotationAmount;
        CursorController.Instance.SetGhostRotation(rotation);
    }
    
}
