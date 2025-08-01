using UnityEngine;
using UnityEngine.Events;

public class BuyableMachineButton : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour buyableBehaviour; // assign a component that implements IBuyable in inspector

    public IBuyable BuyableReference => buyableBehaviour.GetComponent<IBuyable>();

    public UnityEvent<string> OnHoverInfo;

    public void ShowHoverInfo()
    {
        CursorController.Instance.ShowMouseInfo(BuyableReference.GetPrice().ToString(), Color.yellow);
        OnHoverInfo.Invoke(BuyableReference.GetDescription());
    }
    
    public void HoverHide()
    {
        CursorController.Instance.HideMouseInfo();
    }
    
}
