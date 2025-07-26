using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputConsole;
    [SerializeField] Button submitButton;
    private string overrideText;
    public UnityEvent<string> updatedText;
    public UnityEvent onSubmit;
    
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

    public void SubmitPressed()
    {
        submitButton.interactable = false;
        onSubmit.Invoke();
    }

    public void ResetSubmitButton()
    {
        submitButton.interactable = true;
    }
}