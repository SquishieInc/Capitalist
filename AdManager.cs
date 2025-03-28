using UnityEngine;
using UnityEngine.Events;
using IronSourceSDK; // Make sure IronSource is properly integrated

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    [Header("Ad Settings")]
    public string rewardedPlacement = "Rewarded_Video";
    public UnityEvent OnAdAvailable;
    public UnityEvent OnAdUnavailable;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // IronSource initialization
        IronSource.Agent.init("your_app_key", IronSourceAdUnits.REWARDED_VIDEO);
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedAvailabilityChanged;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += OnAdRewarded;
    }

    private void OnDestroy()
    {
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= OnRewardedAvailabilityChanged;
        IronSourceEvents.onRewardedVideoAdRewardedEvent -= OnAdRewarded;
    }

    private void OnRewardedAvailabilityChanged(bool available)
    {
        if (available)
            OnAdAvailable?.Invoke();
        else
            OnAdUnavailable?.Invoke();
    }

    public void ShowRewardedAd(string context = "income_boost")
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo(rewardedPlacement);
            Debug.Log($"[AdManager] Showing rewarded ad for: {context}");
        }
        else
        {
            Debug.LogWarning("[AdManager] No rewarded ad available.");
        }
    }

    private void OnAdRewarded(IronSourcePlacement placement)
    {
        Debug.Log($"[AdManager] Rewarded ad completed: {placement.getRewardName()} x{placement.getRewardAmount()}");

        // Example: Reward based on context, or placement name
        switch (placement.getRewardName().ToLower())
        {
            case "income_boost":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.IncomeMultiplier, 1.0f, 60f); // x2 income for 60 sec
                break;

            case "speed_boost":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.SpeedMultiplier, 1.0f, 30f); // x2 speed for 30 sec
                break;

            case "autocollect":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.AutoCollect, 1.0f, 120f); // auto-run 2 min
                break;

            default:
                // Default reward (or handle gems, etc.)
                CurrencyManager.Instance.AddGems(5);
                break;
        }
    }
}