using UnityEngine;
using UnityEngine.Events;
using IronSourceSDK; // Ensure IronSource plugin is integrated

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    [Header("Ad Settings")]
    public string rewardedPlacement = "Rewarded_Video";
    public UnityEvent OnAdAvailable;
    public UnityEvent OnAdUnavailable;

    private string currentAdContext = "default";

    private void Awake()
    {
        if (Instance == null) Instance = this;

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
        if (AdCooldownManager.Instance.IsOnCooldown(context))
        {
            float remaining = AdCooldownManager.Instance.GetRemainingCooldown(context);
            Debug.LogWarning($"[AdManager] Ad '{context}' is on cooldown ({Mathf.CeilToInt(remaining)}s remaining)");
            return;
        }

        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            currentAdContext = context;
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
        string rewardType = currentAdContext.ToLower();
        Debug.Log($"[AdManager] Rewarded ad completed: {rewardType}");

        switch (rewardType)
        {
            case "income_boost":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.IncomeMultiplier, 1.0f, 60f); // x2 income for 60s
                break;

            case "speed_boost":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.SpeedMultiplier, 1.0f, 30f); // x2 speed for 30s
                break;

            case "autocollect":
                TimedBuffManager.Instance.ApplyBuff(TimedBuffType.AutoCollect, 1.0f, 120f); // auto-collect for 2 mins
                break;

            default:
                CurrencyManager.Instance.AddGems(5); // fallback reward
                break;
        }

        // Start cooldown
        AdCooldownManager.Instance.StartCooldown(rewardType);
    }
}