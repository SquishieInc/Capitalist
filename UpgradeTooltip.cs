using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeTooltip : MonoBehaviour
{
    public static UpgradeTooltip Instance;

    public GameObject tooltipPanel;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Image icon;

    private void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void Show(PrestigeUpgradeSO upgrade, Vector2 screenPosition)
    {
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = screenPosition;

        nameText.text = upgrade.upgradeName;
        descText.text = upgrade.description;
        icon.sprite = upgrade.icon;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
