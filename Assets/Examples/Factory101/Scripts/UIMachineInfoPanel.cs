using TMPro;
using UnityEngine;

public class UIMachineInfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoConsole;
    private string overrideText;
    
    private void Start()
    {
        ClearConsole();
    }

    public void SetText(string text)
    {
        infoConsole.text = text;
    }

    public void SetOverrideText(string text)
    {
        overrideText = text;
        ClearConsole();
    }

    public void ClearConsole()
    {
        infoConsole.text = overrideText;
    }
}
