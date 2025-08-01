using UnityEngine;

public class SplitterBelt : MonoBehaviour, IBuyable
{
    public int basePrice = 20;

    public int GetPrice()
    {
        return basePrice;
    }
}
