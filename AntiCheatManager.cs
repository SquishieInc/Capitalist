using UnityEngine;
using System;
using TMPro;

public class AntiCheatManager : MonoBehaviour
{
    public static AntiCheatManager Instance;

    public double driftThresholdHours = 3;
    public bool flagCheaters = true;

    public GameObject warningPanel;
    public TMP_Text warningText;

    public bool cheatDetected;
    public double detectedDriftHours;

    private void Awake() => Instance = this;

    public bool RunAntiCheatCheck(DateTime serverTime)
    {
        detectedDriftHours = Math.Abs((serverTime - DateTime.UtcNow).TotalHours);

        if (detectedDriftHours > driftThresholdHours)
        {
            cheatDetected = true;
            ShowCheatWarning();
            return true;
        }

        cheatDetected = false;
        return false;
    }

    public double ApplyPenaltyIfCheating(double baseEarnings, bool blockInsteadOfReduce = true)
    {
        if (cheatDetected && flagCheaters)
            return blockInsteadOfReduce ? 0 : baseEarnings * 0.5;

        return baseEarnings;
    }

    private void ShowCheatWarning()
    {
        if (warningPanel != null && warningText != null)
        {
            warningPanel.SetActive(true);
            warningText.text = "⚠ Time manipulation detected.\nOffline rewards limited.";
        }
    }
}
