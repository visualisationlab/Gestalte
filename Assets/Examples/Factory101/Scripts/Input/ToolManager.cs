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

    private Tool currentTool;
    private GameObject currentPlaceablePrefab;
    private float rotation;
    
    public void SelectPlaceableTool(GameObject ghost, GameObject placeablePrefab)
    {
        currentTool = Tool.Place;
        currentPlaceablePrefab = placeablePrefab;
        CursorController.Instance.ShowBuildGhost(ghost);
    }

    public void HidePlaceableTool()
    {
        currentTool = Tool.None;
        currentPlaceablePrefab = null;
        CursorController.Instance.HideBuildGhost();
    }
    
    public void OnClick()
    {
        if (UIUtils.IsPointerOverUI()) return;
        if (InteractionModeController.Instance.CurrentMode != InteractionMode.Placement)
            return;
        
        if (currentTool == Tool.Place)
        {
            PlacePlaceable();
        }

        //TODO DO other Tools
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
        else
        {
            //TODO Cant place
        }
    }
    
    
    
}
