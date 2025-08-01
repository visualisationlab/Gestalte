using Agent;
using TMPro;
using UnityEngine;

public class UIMethodInstructionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI methodName;
    [SerializeField] private TextMeshProUGUI methodInstruction;

    public void SetInstructions(ExposedMethodInterpretation interpretation)
    {
        methodName.text = interpretation.methodNameClean;
        methodInstruction.text = interpretation.description;
    }
}
