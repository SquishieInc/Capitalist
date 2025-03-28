using UnityEngine;
using System;

public class AntiCheatManager : MonoBehaviour
{
    public static AntiCheatManager Instance;

    private bool cheatDetected = false;
    private TimeSpan drift;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RunAntiCheatCheck(DateTime serverTime)
    {
        DateTime localExitTime = DateTime.Parse(PlayerPrefs.GetString("LastExitTime", DateTime.UtcNow.ToString()));
        drift = serverTime - localExitTime;

        if (drift.TotalDays > 3 || drift.TotalSeconds < 0)
        {
            cheatDetected = true;
            Debug.LogWarning("[AntiCheat] Time manipulation detected.");
        }
        else
        {
            cheatDetected = false;
        }
    }

    public double ApplyPenaltyIfCheating(double value, bool isOfflineEarnings)
    {
        if (!cheatDetected) return value;

        float penalty = GameConfigManager.Instance.Config.timeCheatPenaltyMultiplier;
        Debug.LogWarning($"[AntiCheat] Applying {penalty * 100}% penalty to earnings.");
        return value * penalty;
    }

    public bool IsCheating() => cheatDetected;
}
