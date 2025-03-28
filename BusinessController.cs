using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    [Header("Business Data")]
    public BusinessSO businessData;
    public BusinessMilestoneSO milestoneTable;
    public GameBalanceConfigSO gameConfig;

    [Header("Progress")]
    public int level = 0;
    private double currentCost;

    [Header("Manager Settings")]
    public bool managerUnlocked = false;
    public GameObject milestonePopupPrefab;
    public Transform popupAnchor;

    private float lastMilestoneBonus = 1f;

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
            currentCost *= businessData.costMultiplier;

            float newBonus = (float)GetLocalPrestigeMultiplier();
            if (newBonus > lastMilestoneBonus)
            {
                lastMilestoneBonus = newBonus;
                TriggerMilestonePopup(newBonus);
            }
        }
    }

    private void GenerateIncome()
    {
        if (level > 0 && (managerUnlocked || level >= GetManagerUnlockLevel()))
        {
            double baseIncome = businessData.baseIncome * Mathf.Pow(level, businessData.incomeGrowth);

            double prestigeMultiplier = 1.0 + (gameConfig != null ? gameConfig.prestigeMultiplierPerPoint : 0.1) * PrestigeManager.Instance.prestigePoints;
            float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
            double shopMultiplier = 1.0 + shopBoost;
            double localBoost = GetLocalPrestigeMultiplier();

            double finalIncome = baseIncome * prestigeMultiplier * shopMultiplier * localBoost;

            CurrencyManager.Instance.AddCash(finalIncome);
        }
    }

    public double GetCurrentCost() => currentCost;

    public double GetIncomePerCycle()
    {
        double baseIncome = businessData.baseIncome * Mathf.Pow(level, businessData.incomeGrowth);
        double prestigeMultiplier = 1.0 + (gameConfig != null ? gameConfig.prestigeMultiplierPerPoint : 0.1) * PrestigeManager.Instance.prestigePoints;
        float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
        double shopMultiplier = 1.0 + shopBoost;
        double localBoost = GetLocalPrestigeMultiplier();

        return baseIncome * prestigeMultiplier * shopMultiplier * localBoost;
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
        if (autoCollectBoost > 0 && level >= GetManagerUnlockLevel() && !managerUnlocked)
        {
            HireManager();
        }
    }

    private int GetManagerUnlockLevel()
    {
        return gameConfig != null ? gameConfig.defaultManagerUnlockLevel : 25;
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

    private void TriggerMilestonePopup(float newBonus)
    {
        if (milestonePopupPrefab && popupAnchor)
        {
            GameObject popup = Instantiate(milestonePopupPrefab, popupAnchor.position, Quaternion.identity, popupAnchor);
            TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
            if (text) text.text = $"Milestone Reached!\nx{newBonus} Income!";
            Destroy(popup, 2f);
        }

        Debug.Log($"[Milestone] {businessData.businessName} hit x{newBonus} bonus!");
    }
}
