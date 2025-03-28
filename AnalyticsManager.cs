using UnityEngine;
using System;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private DateTime sessionStartTime;
    private int prestigeCountThisSession = 0;

    public event Action<string> OnLog;

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
        string full = $"Event: {eventName} {(string.IsNullOrEmpty(context) ? "" : $"| {context}")}";
        Debug.Log("[Analytics] " + full);
        OnLog?.Invoke(full);
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
