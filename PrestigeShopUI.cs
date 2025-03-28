using UnityEngine;

public class PrestigeShopUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform upgradeListParent;
    public GameObject upgradeButtonPrefab;

    private void Start()
    {
        BuildUpgradeButtons();

        // Subscribe to currency updates
        PrestigeManager.Instance.OnPrestigeCurrencyChanged += RefreshAllUpgradeButtons;
    }

    private void OnDestroy()
    {
        if (PrestigeManager.Instance != null)
            PrestigeManager.Instance.OnPrestigeCurrencyChanged -= RefreshAllUpgradeButtons;
    }

    private void BuildUpgradeButtons()
    {
        // Clear existing buttons
        foreach (Transform child in upgradeListParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and initialize upgrade buttons
        foreach (var upgrade in PrestigeShopManager.Instance.upgrades)
        {
            GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeListParent);
            var buttonUI = buttonObj.GetComponent<PrestigeUpgradeButtonUI>();
            buttonUI.Initialize(upgrade);
        }
    }

    public void RefreshAllUpgradeButtons()
    {
        foreach (Transform child in upgradeListParent)
        {
            var buttonUI = child.GetComponent<PrestigeUpgradeButtonUI>();
            if (buttonUI != null)
                buttonUI.RefreshUI();
        }
    }
}
