using UnityEngine;
using Unity.Services.Mediation;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public string rewardedAdUnitId = "Rewarded_Android";
    private IRewardedAd rewardedAd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private async void Start()
    {
        await Unity.Services.Core.UnityServices.InitializeAsync();
        InitializeRewardedAd();
    }

    private void InitializeRewardedAd()
    {
        rewardedAd = MediationService.Instance.CreateRewardedAd(rewardedAdUnitId);
        rewardedAd.OnLoaded += () => Debug.Log("Rewarded Ad Loaded");
        rewardedAd.OnFailedLoad += (error, message) => Debug.LogError($"Failed to load ad: {message}");
        rewardedAd.OnUserRewarded += OnRewarded;
        rewardedAd.OnClosed += () => Debug.Log("Ad Closed");
        LoadRewardedAd();
    }

    public void LoadRewardedAd() => rewardedAd.Load();

    public void ShowRewardedAd()
    {
        if (rewardedAd.AdState == AdState.Loaded)
            rewardedAd.Show();
        else
        {
            Debug.LogWarning("Rewarded Ad not ready");
            LoadRewardedAd();
        }
    }

    private void OnRewarded(Reward reward)
    {
        Debug.Log("Rewarded Ad Completed - Reward the player.");
        // Reward handled in OfflineEarningsSystem
    }
}
