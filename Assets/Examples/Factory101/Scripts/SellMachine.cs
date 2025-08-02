using UnityEngine;

public class SellMachine : MonoBehaviour, IBuyable
{
    public int basePrice;
    [TextArea (3,12)] public string description;

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
}