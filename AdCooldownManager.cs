using UnityEngine;
using System.Collections.Generic;

public class AdCooldownManager : MonoBehaviour
{
    public static AdCooldownManager Instance;

    [Header("Optional Config")]
    public AdRewardConfigSO config; // Assign this in the Inspector

    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private float defaultCooldownDuration = 300f; // 5 minutes fallback

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Check if an ad reward type is currently on cooldown.
    /// </summary>
    public bool IsOnCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return false;
        return Time.time < cooldowns[adType];
    }

    /// <summary>
    /// Get the remaining cooldown time (in seconds).
    /// </summary>
    public float GetRemainingCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return 0f;
        return Mathf.Max(0f, cooldowns[adType] - Time.time);
    }

    /// <summary>
    /// Starts or resets the cooldown for a specific ad reward type.
    /// </summary>
    public void StartCooldown(string adType)
    {
        float duration = GetConfiguredCooldown(adType);
        cooldowns[adType] = Time.time + duration;

        Debug.Log($"[AdCooldown] Started cooldown for '{adType}' ({duration} seconds)");
    }

    /// <summary>
    /// Fetch the configured cooldown for this reward type from SO.
    /// </summary>
    private float GetConfiguredCooldown(string adType)
    {
        if (config != null)
        {
            foreach (var reward in config.rewards)
            {
                if (reward.rewardID == adType)
                    return reward.cooldownSeconds;
            }
        }

        return defaultCooldownDuration;
    }
}