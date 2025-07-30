using Examples.Factory101.Scripts;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour
{
    public McGibbleDescription description;

    [SerializeField] private TextMeshPro txt;

    private void Start()
    {
        txt.text = description.singleEmoji;
    }
}
