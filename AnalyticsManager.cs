using UnityEngine;
using System;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private DateTime sessionStartTime;
    private int prestigeCountThisSession = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        sessionStartTime = DateTime.UtcNow;
    }

    private void OnApplicationQuit()
    {
        LogSessionDuration();
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            LogSessionDuration();
        }
    }

    public void LogEvent(string eventName, string context = "")
    {
        Debug.Log($"[Analytics] Event: {eventName} {(string.IsNullOrEmpty(context) ? "" : $"| {context}")}");

        // 🔁 Replace with production SDK call if needed
        // FirebaseAnalytics.LogEvent(eventName, new Parameter(...))
    }

    public void LogSessionDuration()
    {
        TimeSpan sessionLength = DateTime.UtcNow - sessionStartTime;
        LogEvent("session_end", $"duration={sessionLength.TotalSeconds:F0}s, prestigeCount={prestigeCountThisSession}");
    }

    public void LogPrestige()
    {
        prestigeCountThisSession++;
        LogEvent("prestige_performed", $"sessionPrestiges={prestigeCountThisSession}");
    }

    public void LogUpgradeEffectiveness(string upgradeName, float boost, string type)
    {
        LogEvent("upgrade_effectiveness", $"name={upgradeName}, boost={boost}, type={type}");
    }
}
