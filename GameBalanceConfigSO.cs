using UnityEngine;

[CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Idle/Game Balance Config")]
public class GameBalanceConfigSO : ScriptableObject
{
    [Header("Prestige Settings")]
    public float prestigeMultiplierPerPoint = 0.1f;

    [Header("Offline Earnings")]
    public float offlineEarningsPenalty = 0.25f;
    public float offlineEarningsBoostCap = 3f;

    [Header("Manager Settings")]
    public float hireManagerCostMultiplier = 5.0f;
    public int defaultManagerUnlockLevel = 25;

    [Header("Upgrade Economics")]
    public float defaultCostMultiplier = 1.15f;
    public float defaultIncomeGrowth = 1.0f;

    [Header("Debug")]
    public bool allowDebugButtons = true;
}
