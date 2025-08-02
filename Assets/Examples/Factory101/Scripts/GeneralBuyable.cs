using Examples.Factory101.Scripts.Input;
using UnityEngine;

public class GeneralBuyable : MonoBehaviour, IBuyable, IBlockPlacement
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
