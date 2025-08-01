using UnityEngine;

public class BuyableMachineButton : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour buyableBehaviour; // assign a component that implements IBuyable in inspector

    public IBuyable BuyableReference => buyableBehaviour as IBuyable;
}
