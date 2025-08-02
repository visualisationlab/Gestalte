using Examples.Factory101.Scripts.Input;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyableMachineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private MonoBehaviour buyableBehaviour; // assign a component that implements IBuyable in inspector

    public IBuyable BuyableReference => buyableBehaviour.GetComponent<IBuyable>();

    public void ShowHoverInfo()
    {
        CursorController.Instance.ShowMouseInfo(BuyableReference.GetPrice().ToString(), Color.yellow);
        GlobalMenuManager.Instance.sideInfoText.text = BuyableReference.GetDescription();
    }
    
    public void HoverHide()
    {
        CursorController.Instance.HideMouseInfo();
        GlobalMenuManager.Instance.sideInfoText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHoverInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverHide();
    }
}
