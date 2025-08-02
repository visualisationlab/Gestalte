using Examples.Factory101.Scripts.Input;
using UnityEngine;

public class SplitterBelt : MonoBehaviour, IBuyable, IBlockPlacement
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
