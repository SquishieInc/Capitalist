using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    public BusinessSO businessData;
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
        // Only run if level > 0 AND either: manually triggered or automated by manager
        if (level > 0 && (isAutoCollecting || level >= unlockManagerAtLevel))
        {
            double baseIncome = businessData.baseIncome * Mathf.Max(level, 1);

            double prestigeMultiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);
            float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
            double shopMultiplier = 1.0 + shopBoost;
            double localPrestigeBoost = GetLocalPrestigeMultiplier(); // stub for future

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
            HireManager(); // Optional auto-unlock if desired
        }
    }

    private double GetLocalPrestigeMultiplier()
    {
        // Placeholder for future BusinessMilestoneSO
        return 1.0;
    }
}
