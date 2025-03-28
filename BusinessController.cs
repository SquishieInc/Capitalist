using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    public BusinessSO businessData;
    public int level = 0;
    private double currentCost;

    [Header("Manager Settings")]
    public bool managerUnlocked = false;
    public int unlockManagerAtLevel = 25; // Customize per business
    public bool isAutoCollecting => managerUnlocked;

    public BusinessController controller;
    public GameObject hireButton;

    private void Start()
    {
        currentCost = businessData.baseCost;
        InvokeRepeating(nameof(GenerateIncome), businessData.incomeInterval, businessData.incomeInterval);
        CheckAutoCollect();
    }

    private void Update()
    {
        if (controller.managerUnlocked)
        {
            hireButton.SetActive(false);
        }
        else
        {
            hireButton.SetActive(controller.level >= controller.unlockManagerAtLevel);
        }
    }

        public void OnHireButtonClicked()
    {
        controller.HireManager();
        hireButton.SetActive(false);
    }
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
    if (level > 0 && (isAutoCollecting || level >= unlockManagerAtLevel))
    {
        double baseIncome = businessData.baseIncome * Mathf.Max(level, 1);

        double prestigeMultiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);
        float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
        double shopMultiplier = 1.0 + shopBoost;

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

    public void HireManager()
{
    if (managerUnlocked) return;

    managerUnlocked = true;
    Debug.Log($"Manager hired for {businessData.businessName}!");
}
}
