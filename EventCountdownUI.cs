using UnityEngine;
using TMPro;
using System;

public class EventCountdownUI : MonoBehaviour
{
    public TMP_Text countdownText;
    private DateTime eventEndTime;

    private void Start()
    {
        if (RemoteEventController.Instance.eventActive)
            eventEndTime = DateTime.Now.AddHours(RemoteEventController.Instance.eventDurationHours);
    }

    private void Update()
    {
        if (!RemoteEventController.Instance.eventActive) return;

        TimeSpan timeLeft = eventEndTime - DateTime.Now;
        countdownText.text = timeLeft.TotalSeconds > 0
            ? $"Ends in: {timeLeft.Hours}h {timeLeft.Minutes}m"
            : "Event Over";
    }
}
