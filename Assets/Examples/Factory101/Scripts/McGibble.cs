using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour, IHoverable
{
    public McGibbleDescription description;
    public int heat;
    public Color sprayColor = Color.white;
    public FinishType finishType = FinishType.Mat;
    
    [Header("Finish multipliers")]
    [SerializeField] private float matFinishMultiplier = 1f;
    [SerializeField] private float shinyFinishMultiplier = 1.5f;
    [SerializeField] private float metallicFinishMultiplier = 2f;
    [SerializeField] private float glossyFinishMultiplier = 3f;
    [SerializeField] private float pearlFinishMultiplier = 4f;
    [SerializeField] private float galacticFinishMultiplier = 10f;
    [SerializeField] private float unknownFinishMultiplier = 1f;
    [SerializeField] private TextMeshPro txt;

    // Multipliers per category; tweakable in inspector
    [Header("Heat Multipliers")]
    [SerializeField] private float frozenMultiplier = 1f;
    [SerializeField] private float normalMultiplier = 1f;
    [SerializeField] private float warmMultiplier = 1.2f;
    [SerializeField] private float hotMultiplier = 2.5f;
    [SerializeField] private float wellDoneMultiplier = 10f;
    [SerializeField] private float crispyMultiplier = 5f;
    [SerializeField] private float burntMultiplier = 0.5f;
    [SerializeField] private float unknownMultiplier = 1f;

    
    private void Start()
    {
        txt.text = description.singleEmoji;
        finishType = FinishType.Mat; // Default finish type
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
            HeatCategory.WellDone => "Well Done",
            HeatCategory.Burnt => "Burnt",
            _ => "Unknown"
        };
    }

    public string GetFullDescription()
    {
        string result = "";
        result += $"<b>{description.name}</b>\n";
        result += $"The temperature is {GetHeatDescription()}\n";
        result += $"The finish is {getFinishDescription()}\n";
        result += $"The Sale price is {GetCurrentSalePrice()}\n";
        return result;
    }

    private HeatCategory GetHeatCategory()
    {
        if (heat < 0) return HeatCategory.Frozen;
        if (heat <= 30) return HeatCategory.Normal;
        if (heat <= 70) return HeatCategory.Warm;
        if (heat <= 130) return HeatCategory.Hot;
        if (heat <= 160) return HeatCategory.WellDone;
        if (heat <= 220) return HeatCategory.Crispy;
        if (heat > 220) return HeatCategory.Burnt;
        return HeatCategory.Unknown;
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
            HeatCategory.WellDone => wellDoneMultiplier,
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
        WellDone,
        Burnt,
        Unknown
    }

    public float GetFinishMultiplier()
    {
        return finishType switch
        {
            FinishType.Mat => matFinishMultiplier,
            FinishType.Shiny => shinyFinishMultiplier,
            FinishType.Metallic => metallicFinishMultiplier,
            FinishType.Glossy => glossyFinishMultiplier,
            FinishType.Pearl => pearlFinishMultiplier,
            FinishType.Galactic => galacticFinishMultiplier,
            _ => unknownFinishMultiplier
        };
    }

    private string getFinishDescription()
    {
        return finishType switch
        {
            FinishType.Mat => "Matte",
            FinishType.Shiny => "Shiny",
            FinishType.Metallic => "Metallic",
            FinishType.Glossy => "Glossy",
            FinishType.Pearl => "Pearlescent",
            FinishType.Galactic => "Galactic",
            _ => "Unknown"
        };
    }
    public void SetFinishType(FinishType newFinish)
    {
        finishType = newFinish;
    }

    public float GetTotalMultiplier()
    {
        return GetHeatMultiplier() * GetFinishMultiplier();
    }
    public int GetCurrentSalePrice()
    {
        float basePrice = description.raritySalePrice; // keep existing field name
        float multiplier = GetTotalMultiplier();
        return Mathf.RoundToInt(basePrice * multiplier);
    }
    public void OnHoverEnter()
    {
        GlobalMenuManager.Instance.sideInfoText.text = GetFullDescription();
    }

    public void OnHoverExit()
    {
        GlobalMenuManager.Instance.sideInfoText.text = "";
    }

    public void SetTemperature(int temperature)
    {
        heat = temperature;
    }

    public void OnStartDrag()
    {
        //TODO: Show arrow on mans mouth
    }

    public void OnStopDrag()
    {
        //TODO: Hide arrow on mans mouth
    }
}
