using System;
using UnityEngine;
using UnityEngine.Events;

public class ToolManager : MonoBehaviour
{
    [Serializable]
    public enum Tool
    {
        None,
        Place
    }
    [SerializeField] public PointerRaycaster pointerRaycaster;
    private Tool currentTool;
    private GameObject currentPlaceablePrefab;
    private float rotation;
    private float rotationAmount = 90f;

    public UnityEvent OnRanOutOfFunds;

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
            if (!HadEnoughFunds())
            {
                HidePlaceableTool();
                InteractionModeController.Instance.SetModeDefault();
                OnRanOutOfFunds.Invoke();
            }
        }
    }

    public void PlacePlaceable()
    {
        if (HadEnoughFunds())
        {
            var buyable = currentPlaceablePrefab.GetComponent<IBuyable>();
            var realPlaceable = Instantiate(currentPlaceablePrefab, CursorController.Instance.GetPositionInGrid(), Quaternion.identity);
            realPlaceable.transform.Rotate(Vector3.forward, rotation);
            AudioManager.Instance.Pluck();
            GameInfoManager.Instance.AddMoney(-buyable.GetPrice());
        }
    }

    private bool HadEnoughFunds()
    {
        var buyable = currentPlaceablePrefab.GetComponent<IBuyable>();
        return (buyable.GetPrice() <= GameInfoManager.Instance.GetMoney());
    }

    public void Rotate()
    {
        rotation += rotationAmount;
        CursorController.Instance.SetGhostRotation(rotation);
    }
    
}
