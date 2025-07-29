using System;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour
{
    public string icon;
    public int salePrice;

    [SerializeField] private TextMeshPro txt;
    private void Start()
    {
        txt.text = icon;
    }
}
