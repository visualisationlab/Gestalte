using Examples.Factory101.Scripts.Input;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyableMachineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private MonoBehaviour buyableBehaviour; // assign a component that implements IBuyable in inspector

    public IBuyable BuyableReference => buyableBehaviour.GetComponent<IBuyable>();

    public bool isFree;

    public void ShowHoverInfo()
    {
        if (isFree) return;
        CursorController.Instance.ShowMouseInfo(BuyableReference.GetPrice().ToString(), Color.yellow);
        GlobalMenuManager.Instance.sideInfoText.text = BuyableReference.GetDescription();
    }
    
    public void HoverHide()
    {
        if (isFree) return;
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
