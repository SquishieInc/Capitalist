using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    public BusinessSO businessData;
    public int level = 0;
    private double currentCost;

    private void Start()
    {
        currentCost = businessData.baseCost;
        InvokeRepeating(nameof(GenerateIncome), businessData.incomeInterval, businessData.incomeInterval);
        CheckAutoCollect();
    }

    public void LevelUp()
    {
        if (CurrencyManager.Instance.SpendCash(currentCost))
        {
            level++;
            currentCost *= 1.15;
        }
    }

    private void GenerateIncome()
    {
        if (level > 0)
        {
            double baseIncome = businessData.baseIncome * level;

            // 🧠 Prestige Points bonus
            double prestigeMultiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);

            // 🛍️ Prestige Shop income bonus
            float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
            double shopMultiplier = 1.0 + shopBoost;

            double finalIncome = baseIncome * prestigeMultiplier * shopMultiplier;

            CurrencyManager.Instance.AddCash(finalIncome);
        }
    }

    public double GetCurrentCost() => currentCost;

    public double GetIncomePerCycle()
    {
        double baseIncome = businessData.baseIncome * level;
        double prestigeMultiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);
        float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
        double shopMultiplier = 1.0 + shopBoost;

        return baseIncome * prestigeMultiplier * shopMultiplier;
    }

    public void OnPrestigeReset()
    {
        level = 0;
        currentCost = businessData.baseCost;
    }

    private void CheckAutoCollect()
    {
        float autoCollectBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.AutoCollect);
        if (autoCollectBoost > 0)
        {
            Debug.Log($"[AutoCollect] Enabled for {businessData.businessName}");
            // Future behavior: auto-level unlock or background generation
        }
    }
}
