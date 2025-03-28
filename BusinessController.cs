using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    [Header("Business Data")]
    public BusinessSO businessData;
    public BusinessMilestoneSO milestoneTable;

    [Header("Progress")]
    public int level = 0;
    private double currentCost;

    [Header("Manager Settings")]
    public bool managerUnlocked = false;
    public int unlockManagerAtLevel = 25;
    public bool isAutoCollecting => managerUnlocked;

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
        // Business must be active OR auto-collecting (manager hired or milestone reached)
        if (level > 0 && (isAutoCollecting || level >= unlockManagerAtLevel))
        {
            double baseIncome = businessData.baseIncome * Mathf.Max(level, 1);

            // Global prestige point multiplier
            double prestigeMultiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);

            // Prestige Shop upgrade multiplier
            float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
            double shopMultiplier = 1.0 + shopBoost;

            // Local business milestone multiplier
            double localPrestigeBoost = GetLocalPrestigeMultiplier();

            double finalIncome = baseIncome * prestigeMultiplier * shopMultiplier * localPrestigeBoost;

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
        double localPrestigeBoost = GetLocalPrestigeMultiplier();

        return baseIncome * prestigeMultiplier * shopMultiplier * localPrestigeBoost;
    }

    public void OnPrestigeReset()
    {
        level = 0;
        currentCost = businessData.baseCost;
        managerUnlocked = false;
    }

    public void HireManager()
    {
        if (!managerUnlocked)
        {
            managerUnlocked = true;
            Debug.Log($"Manager hired for {businessData.businessName}!");
        }
    }

    private void CheckAutoCollect()
    {
        float autoCollectBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.AutoCollect);
        if (autoCollectBoost > 0 && level >= unlockManagerAtLevel && !managerUnlocked)
        {
            HireManager(); // Optional: automatically hires manager when conditions are met
        }
    }

    private double GetLocalPrestigeMultiplier()
    {
        if (milestoneTable == null || milestoneTable.milestones.Count == 0)
            return 1.0;

        float highestBonus = 1.0f;

        foreach (var milestone in milestoneTable.milestones)
        {
            if (level >= milestone.requiredLevel && milestone.incomeMultiplier > highestBonus)
            {
                highestBonus = milestone.incomeMultiplier;
            }
        }

        return highestBonus;
    }
}
