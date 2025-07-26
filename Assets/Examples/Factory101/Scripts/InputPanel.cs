using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputConsole;
    private string overrideText;
    public UnityEvent<string> updatedText;

    private void Start()
    {
        ClearConsole();
    }

    public void SetText(string text)
    {
        inputConsole.text = text;
    }

    public void SetOverrideText(string text)
    {
        overrideText = text;
        ClearConsole();
    }

    public void ClearConsole()
    {
        inputConsole.text = overrideText;
    }

    public void UpdateText()
    {
        updatedText.Invoke(inputConsole.text);
    }
}