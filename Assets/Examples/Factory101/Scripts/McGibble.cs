using Examples.Factory101.Scripts;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour
{
    public McGibbleDescription description;
    public int heat;
    [SerializeField] private TextMeshPro txt;

    // Multipliers per category; tweakable in inspector
    [Header("Heat Multipliers")]
    [SerializeField] private float frozenMultiplier = 1f;
    [SerializeField] private float normalMultiplier = 1f;
    [SerializeField] private float warmMultiplier = 1.2f;
    [SerializeField] private float hotMultiplier = 1.5f;
    [SerializeField] private float crispyMultiplier = 10f;
    [SerializeField] private float burntMultiplier = 0.5f;
    [SerializeField] private float unknownMultiplier = 1f;

    private void Start()
    {
        txt.text = description.singleEmoji;
    }

    private string GetHeatDescription()
    {
        return GetHeatCategory() switch
        {
            HeatCategory.Frozen => "Frozen",
            HeatCategory.Normal => "Normal",
            HeatCategory.Warm => "Warm",
            HeatCategory.Hot => "Hot",
            HeatCategory.Crispy => "Crispy",
            HeatCategory.Burnt => "Burnt",
            _ => "Unknown"
        };
    }

    public string GetFullDescription()
    {
        return $"The temperature is {GetHeatDescription()}";
    }

    public int GetCurrentSalePrice()
    {
        float basePrice = description.raritySalePrice; // keep existing field name
        float multiplier = GetHeatMultiplier();
        return Mathf.RoundToInt(basePrice * multiplier);
    }

    private HeatCategory GetHeatCategory()
    {
        if (heat < 0) return HeatCategory.Frozen;
        if (heat <= 30) return HeatCategory.Normal;
        if (heat <= 60) return HeatCategory.Warm;
        if (heat <= 110) return HeatCategory.Hot;
        if (heat <= 160) return HeatCategory.Crispy;
        if (heat > 200) return HeatCategory.Burnt;
        return HeatCategory.Unknown; // covers 161..200
    }

    private float GetHeatMultiplier()
    {
        return GetHeatCategory() switch
        {
            HeatCategory.Frozen => frozenMultiplier,
            HeatCategory.Normal => normalMultiplier,
            HeatCategory.Warm => warmMultiplier,
            HeatCategory.Hot => hotMultiplier,
            HeatCategory.Crispy => crispyMultiplier,
            HeatCategory.Burnt => burntMultiplier,
            _ => unknownMultiplier
        };
    }

    private enum HeatCategory
    {
        Frozen,
        Normal,
        Warm,
        Hot,
        Crispy,
        Burnt,
        Unknown
    }
}
