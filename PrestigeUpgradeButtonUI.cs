using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrestigeUpgradeButtonUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text upgradeNameText;
    public TMP_Text upgradeDescText;
    public TMP_Text upgradeLevelText;
    public TMP_Text upgradeCostText;
    public Image upgradeIcon;
    public Button buyButton;

    [Header("Upgrade Data")]
    public PrestigeUpgradeSO upgradeData;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
        RefreshUI();
    }

    public void Initialize(PrestigeUpgradeSO data)
    {
        upgradeData = data;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (upgradeData == null) return;

        int level = PrestigeShopManager.Instance.GetUpgradeLevel(upgradeData);
        double cost = upgradeData.GetCostForLevel(level);

        upgradeNameText.text = upgradeData.upgradeName;
        upgradeDescText.text = upgradeData.description;
        upgradeLevelText.text = $"Lv. {level}";
        upgradeCostText.text = $"Cost: {cost:0}";
        upgradeIcon.sprite = upgradeData.icon;

        bool affordable = PrestigeManager.Instance.unspentPrestigeCurrency >= cost;
        buyButton.interactable = affordable;
    }

    private void OnBuyClicked()
    {
        PrestigeShopManager.Instance.BuyUpgrade(upgradeData);
        RefreshUI();
    }
}
