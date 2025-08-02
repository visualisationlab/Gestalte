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
    }
    
    public void HoverHide()
    {
        CursorController.Instance.HideMouseInfo();
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
