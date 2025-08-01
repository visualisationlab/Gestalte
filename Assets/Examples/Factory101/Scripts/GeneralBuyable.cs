using UnityEngine;

public class GeneralBuyable : MonoBehaviour, IBuyable
{
    public int basePrice;

    public int GetPrice()
    {
        return basePrice;
    }
}
