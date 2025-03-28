using UnityEngine;
using System.Collections.Generic;

public class PrestigeShopManager : MonoBehaviour
{
    public static PrestigeShopManager Instance;

    [Header("Prestige Upgrades")]
    public List<PrestigeUpgradeSO> upgrades; // Manually ordered in Inspector
    private Dictionary<PrestigeUpgradeSO, int> upgradeLevels = new Dictionary<PrestigeUpgradeSO, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        LoadUpgradeProgress(); // Optional fallback
    }

    public int GetUpgradeLevel(PrestigeUpgradeSO upgrade)
    {
        return upgradeLevels.ContainsKey(upgrade) ? upgradeLevels[upgrade] : 0;
    }

    public void SetUpgradeLevel(PrestigeUpgradeSO upgrade, int level)
    {
        if (upgradeLevels.ContainsKey(upgrade))
            upgradeLevels[upgrade] = level;
        else
            upgradeLevels.Add(upgrade, level);
    }

    public float GetTotalEffect(PrestigeUpgradeSO.UpgradeType effectType)
    {
        float total = 0f;

        foreach (var upgrade in upgrades)
        {
            if (upgrade.effectType == effectType)
            {
                int level = GetUpgradeLevel(upgrade);
                total += upgrade.baseValue * level;
            }
        }

        return total;
    }

    public void BuyUpgrade(PrestigeUpgradeSO upgrade)
    {
        int currentLevel = GetUpgradeLevel(upgrade);
        double cost = upgrade.GetCostForLevel(currentLevel);

        if (PrestigeManager.Instance.unspentPrestigeCurrency >= cost)
        {
            PrestigeManager.Instance.SpendPrestigeCurrency(cost);
            SetUpgradeLevel(upgrade, currentLevel + 1);

            // ✅ Auto-save after purchase
            SaveSystem.Instance.SaveGame();

            // 📊 Analytics hook
            AnalyticsManager.Instance.LogEvent("prestige_upgrade_bought", $"name={upgrade.upgradeName}, level={currentLevel + 1}");
        }
    }

    private void LoadUpgradeProgress()
    {
        foreach (var upgrade in upgrades)
        {
            string key = $"prestige_upgrade_{upgrade.upgradeName}";
            int level = PlayerPrefs.GetInt(key, 0);
            SetUpgradeLevel(upgrade, level);
        }
    }
}
