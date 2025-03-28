using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public TMP_Text cashText;
    public TMP_Text gemsText;
    public TMP_Text totalEarnedText;

    private void Update()
    {
        cashText.text = $"Cash: ${CurrencyManager.Instance.cash:0}";
        gemsText.text = $"Gems: {CurrencyManager.Instance.gems} 💎";
        totalEarnedText.text = $"Total Earned: ${CurrencyManager.Instance.totalCashEarned:0}";
    }
}
