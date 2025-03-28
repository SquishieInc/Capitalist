using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DevAnalyticsOverlay : MonoBehaviour
{
    public GameObject panelRoot;
    public TMP_Text logText;
    public int maxLines = 30;

    private Queue<string> logLines = new Queue<string>();

    private void Awake()
    {
        panelRoot.SetActive(Debug.isDebugBuild); // Only show in dev builds
        AnalyticsManager.Instance.OnLog += AddLogLine;
    }

    public void AddLogLine(string line)
    {
        if (logLines.Count >= maxLines)
            logLines.Dequeue();

        logLines.Enqueue($"[{System.DateTime.Now:HH:mm:ss}] {line}");

        logText.text = string.Join("\n", logLines);
    }

    private void OnDestroy()
    {
        if (AnalyticsManager.Instance != null)
            AnalyticsManager.Instance.OnLog -= AddLogLine;
    }
}
