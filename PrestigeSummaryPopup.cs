using UnityEngine;
using TMPro;

public class PrestigeSummaryPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text summaryText;

    public void Show(int amount)
    {
        popupPanel.SetActive(true);
        summaryText.text = $"+{amount} Prestige Gained!";
        Invoke(nameof(Hide), 2.5f);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }
}
