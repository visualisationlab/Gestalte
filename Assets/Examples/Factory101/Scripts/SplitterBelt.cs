using UnityEngine;

public class SplitterBelt : MonoBehaviour, IBuyable
{
    public int basePrice = 20;
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
