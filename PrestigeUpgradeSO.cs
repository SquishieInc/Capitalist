using UnityEngine;

[CreateAssetMenu(fileName = "NewPrestigeUpgrade", menuName = "Idle/Prestige Upgrade")]
public class PrestigeUpgradeSO : ScriptableObject
{
    public enum UpgradeType
    {
        IncomeMultiplier,
        OfflineEarningsBoost,
        AutoCollect,
        SpeedBoost,
        ManagerUnlock,
        Special
    }

    public enum UpgradeCategory
    {
        Economy,
        Automation,
        Utility,
        Visual,
        LimitedTime
    }

    [Header("Upgrade Info")]
    public string upgradeName;
    [TextArea]
    public string description;

    public Sprite icon;
    public UpgradeCategory category;
    public UpgradeType effectType;

    [Header("Upgrade Effects")]
    public float baseValue = 0.1f;        // e.g., +10% per level
    public float baseCost = 10f;          // base prestige cost
    public float costMultiplier = 2.0f;   // cost scaling per level

    public double GetCostForLevel(int level)
    {
        return baseCost * Mathf.Pow(costMultiplier, level);
    }
}
