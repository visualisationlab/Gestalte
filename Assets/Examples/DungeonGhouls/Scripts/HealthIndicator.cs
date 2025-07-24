using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    public Sprite fullHearthSprite;
    public Sprite emptyHearthSprite;
    public List<Image> hearthIndicators;

    private void Start()
    {
        ResetHearths();
    }

    private void ResetHearths()
    {
        foreach (var hearthIndicator in hearthIndicators)
        {
            hearthIndicator.sprite = fullHearthSprite;
        }
    }
    public void SetHealth(int health)
    {
        for (int i = hearthIndicators.Count-1; i >= health; i--)
        {
            hearthIndicators[i].sprite = emptyHearthSprite;
        }
    }
    
    [ContextMenu("Test Set Health")]
    public void TestHealth()
    {
        SetHealth(1);
    }
}
