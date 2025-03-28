using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void LogEvent(string eventName, string context = "")
    {
        Debug.Log($"[Analytics] Event: {eventName} {context}");

        // 🔁 Replace with real analytics SDK call if needed
        // e.g., FirebaseAnalytics.LogEvent(eventName, new Parameter(...))
    }
}
