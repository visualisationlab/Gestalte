using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoConsole;
    private string overrideText;
    
    private void Start()
    {
        ClearConsole();
    }
    
    public void HoverGameObject(GameObject go)
    {
        var machineDescription = go.GetComponent<ExposeMachine>();
        if (machineDescription == null) return;
        var methods = machineDescription.GetExposedMethodsNames();
        infoConsole.text = machineDescription.description + "\nFunctions: \n" + methods;
    }

    public void SelectGameObject(GameObject go)
    {
        var machineDescription = go.GetComponent<ExposeMachine>();
        if (machineDescription == null) return;
        SetOverrideText(machineDescription.description);
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
