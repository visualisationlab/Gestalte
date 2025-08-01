using UnityEngine;

public class GeneralBuyable : MonoBehaviour, IBuyable
{
    public int basePrice;
    [TextArea] public string description;

    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }
}
