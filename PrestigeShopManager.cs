using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PrestigeShopManager : MonoBehaviour
{
    public Transform upgradeListParent;
    public GameObject upgradeUIPrefab;
    public List<PrestigeUpgradeSO> upgrades;

    private Dictionary<PrestigeUpgradeSO, int> upgradeLevels = new();

    private void Start()
    {
        LoadUpgradeProgress();
        RenderUpgradeList();
    }

    private void RenderUpgradeList()
    {
        foreach (var upgrade in upgrades)
        {
            GameObject ui = Instantiate(upgradeUIPrefab, upgradeListParent);
            ui.GetComponentInChildren<TMP_Text>().text = $"{upgrade.upgradeName}\nLv {GetLevel(upgrade)}/{upgrade.maxLevel}";
            ui.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                TryPurchase(upgrade);
            });
        }
    }

    private void TryPurchase(PrestigeUpgradeSO upgrade)
    {
        if (GetLevel(upgrade) >= upgrade.maxLevel) return;
        if (PrestigeManager.Instance.unspentPrestigeCurrency < upgrade.cost) return;

        PrestigeManager.Instance.SpendPrestigeCurrency(upgrade.cost);
        upgradeLevels[upgrade] = GetLevel(upgrade) + 1;

        SaveUpgradeProgress();
        Debug.Log($"Purchased: {upgrade.upgradeName} → Level {GetLevel(upgrade)}");
    }

    private int GetLevel(PrestigeUpgradeSO upgrade)
    {
        return upgradeLevels.ContainsKey(upgrade) ? upgradeLevels[upgrade] : 0;
    }

    private void SaveUpgradeProgress()
    {
        // Optional: Extend SaveSystem to store levels
        // Or save to PlayerPrefs for now
    }

    private void LoadUpgradeProgress()
    {
        // Optional: Load from SaveSystem or PlayerPrefs
    }

    public float GetTotalEffect(PrestigeUpgradeSO.UpgradeType type)
    {
        float total = 0;
        foreach (var kv in upgradeLevels)
        {
            if (kv.Key.upgradeType == type)
            {
                total += kv.Value * kv.Key.valuePerLevel;
            }
        }
        return total;
    }
}
