using TMPro;
using UnityEngine;

public class UIGameInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI moneyTxt;

    public void SetMoneyText(int amount)
    {
        moneyTxt.text = $"${amount.ToString()}";
    }
}
