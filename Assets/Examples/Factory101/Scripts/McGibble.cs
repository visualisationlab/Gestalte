using System;
using TMPro;
using UnityEngine;

public class McGibble : MonoBehaviour
{
    public int salePrice;
    public string gibbleType;
    public int heat;

    [SerializeField] private TextMeshPro txt;

    private void Start()
    {
        txt.text = gibbleType;
    }
}
