using UnityEngine;

[CreateAssetMenu(fileName = "NewPrestigeUpgrade", menuName = "Idle/Prestige Upgrade")]
public class PrestigeUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string description;
    public double cost;
    public int maxLevel = 1;
    public Sprite icon;

    [Header("Effect")]
    public UpgradeType upgradeType;
    public float valuePerLevel = 0.1f;

    public enum UpgradeType
    {
        IncomeMultiplier,
        OfflineEarningsBoost,
        AdRewardMultiplier,
        AutoCollect
        // Add more effect types here
    }
}
