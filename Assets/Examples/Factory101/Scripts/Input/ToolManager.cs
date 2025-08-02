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
    
    public void OnClick()
    {
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
        Debug.Log($"BUYABLE: {buyable}");
        
        if (buyable.GetPrice() <= GameInfoManager.Instance.GetMoney())
        {
            var realPlaceable = Instantiate(currentPlaceablePrefab, transform.position, Quaternion.identity);
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
