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
        var config = GameConfigManager.Instance.Config;

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
        var config = GameConfigManager.Instance.Config;

        if (level > 0 && (managerUnlocked || level >= config.defaultManagerUnlockLevel))
        {
            double baseIncome = businessData.baseIncome * Mathf.Pow(level, businessData.incomeGrowth);
            double prestigeMultiplier = 1.0 + config.prestigeMultiplierPerPoint * PrestigeManager.Instance.prestigePoints;
            float shopBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.IncomeMultiplier);
            double shopMultiplier = 1.0 + shopBoost;
            double localBoost = GetLocalPrestigeMultiplier();

            double finalIncome = baseIncome * prestigeMultiplier * shopMultiplier * localBoost;

            CurrencyManager.Instance.AddCash(finalIncome);
        }
    }

    public void HireManager()
    {
        if (!managerUnlocked)
        {
            managerUnlocked = true;
            Debug.Log($"Manager hired for {businessData.businessName}!");
        }
    }

    public double GetCurrentCost() => currentCost;

    public double GetIncomePerCycle()
    {
        var config = GameConfigManager.Instance.Config;

        double baseIncome = businessData.baseIncome * Mathf.Pow(level, businessData.incomeGrowth);
        double prestigeMultiplier = 1.0 + config.prestigeMultiplierPerPoint * PrestigeManager.Instance.prestigePoints;
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
        lastMilestoneBonus = 1f;
    }

    private void CheckAutoCollect()
    {
        float autoCollectBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.AutoCollect);
        var config = GameConfigManager.Instance.Config;

        if (autoCollectBoost > 0 && level >= config.defaultManagerUnlockLevel && !managerUnlocked)
        {
            HireManager();
        }
    }

    public double GetLocalPrestigeMultiplier()
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
        var milestone = milestoneTable.milestones.Find(m => m.requiredLevel == level);
        if (milestone == null) return;

        // Display milestone popup
        if (milestonePopupPrefab && popupAnchor)
        {
            GameObject popup = Instantiate(milestonePopupPrefab, popupAnchor.position, Quaternion.identity, popupAnchor);
            TMPro.TMP_Text text = popup.GetComponentInChildren<TMPro.TMP_Text>();
            if (text) text.text = $"Milestone Reached!\nx{newBonus} Income!";
            Destroy(popup, 2f);
        }

        // VFX + SFX
        VFXManager.Instance?.PlayVFX(VFXManager.Instance.milestoneFlash, popupAnchor.position);
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.milestoneSFX);

        // Trigger milestone effect
        switch (milestone.effectType)
        {
            case MilestoneEffectType.AutoCollect:
                HireManager();
                break;

            case MilestoneEffectType.GemsReward:
                CurrencyManager.Instance.AddGems(milestone.effectValue);
                break;

            case MilestoneEffectType.SpeedBoost:
                // Placeholder: could scale income interval for X seconds
                Debug.Log($"[Milestone] Speed boost triggered (not implemented)");
                break;

            case MilestoneEffectType.PlayEffect:
                Debug.Log($"[Milestone] Visual effect trigger (handled via VFXManager)");
                break;
        }

        Debug.Log($"[Milestone] {businessData.businessName} hit {milestone.effectType} effect!");
    }
}