using UnityEngine;

[CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Idle/Game Balance Config")]
public class GameBalanceConfigSO : ScriptableObjectect
{
    [Header("Prestige Settings")]
    public float prestigeMultiplierPerPoint = 0.1f;
    public double prestigeBaseDivisor = 1_000_000; // NEW

    [Header("Offline Earnings")]
    public float offlineEarningsPenalty = 0.25f;
    public float offlineEarningsBoostCap = 3f;
    public float baseOfflineEarningsRate = 10f; 
    public float offlineEarningsCapHours = 8f;

    [Header("Manager Settings")]
    public float hireManagerCostMultiplier = 5.0f;
    public int defaultManagerUnlockLevel = 25;

    [Header("Upgrade Economics")]
    public float defaultCostMultiplier = 1.15f;
    public float defaultIncomeGrowth = 1.0f;

    [Header("Anti-Cheat")]
    public float timeCheatPenaltyMultiplier = 0.25f; // NEW

    [Header("Debug")]
    public bool allowDebugButtons = true;
}
