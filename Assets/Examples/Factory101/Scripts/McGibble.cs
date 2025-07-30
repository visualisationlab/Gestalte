using Examples.Factory101.Scripts;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour
{
    public McGibbleDescription description;
    public int heat;
    [SerializeField] private TextMeshPro txt;

    private void Start()
    {
        txt.text = description.singleEmoji;
    }

    private string GetHeatDescription()
    {
        return heat switch
        {
            < 0           => "Frozen",
            <= 30         => "Normal",
            <= 60         => "Warm",
            <= 110        => "Hot",
            <= 160        => "Crispy",
            > 200         => "Burnt",
            _             => "unknown"   // covers 161..200
        };
    }

    public string GetFullDescription()
    {
        return $"The temperature is {GetHeatDescription()}";
    }
}
