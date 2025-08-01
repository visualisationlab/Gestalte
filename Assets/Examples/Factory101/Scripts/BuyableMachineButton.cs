using UnityEngine;

public class BuyableMachineButton : MonoBehaviour
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
    
}
