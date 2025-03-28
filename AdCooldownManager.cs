using UnityEngine;
using System.Collections.Generic;

public class AdCooldownManager : MonoBehaviour
{
    public static AdCooldownManager Instance;

    [Header("Optional Config")]
    public AdRewardConfigSO config; // Assign this in the Inspector

    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private float defaultCooldownDuration = 300f; // 5 minutes fallback

    private const string COOLDOWN_KEY = "AdCooldownData";

    [System.Serializable]
    public class CooldownSaveData
    {
        public List<string> ids = new List<string>();
        public List<float> endTimes = new List<float>();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        LoadCooldowns();
    }

    private void OnApplicationQuit()
    {
        SaveCooldowns();
    }

    /// <summary>
    /// Check if a specific ad reward type is on cooldown.
    /// </summary>
    public bool IsOnCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return false;
        return Time.time < cooldowns[adType];
    }

    /// <summary>
    /// Get the remaining cooldown (in seconds) for an ad type.
    /// </summary>
    public float GetRemainingCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return 0f;
        return Mathf.Max(0f, cooldowns[adType] - Time.time);
    }

    /// <summary>
    /// Start or refresh a cooldown for a given ad reward type.
    /// </summary>
    public void StartCooldown(string adType)
    {
        float duration = GetConfiguredCooldown(adType);
        cooldowns[adType] = Time.time + duration;

        Debug.Log($"[AdCooldown] Started cooldown for '{adType}' ({duration} sec)");
        SaveCooldowns();
    }

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

    public void SaveCooldowns()
    {
        CooldownSaveData data = new CooldownSaveData();

        foreach (var kvp in cooldowns)
        {
            data.ids.Add(kvp.Key);
            data.endTimes.Add(kvp.Value);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(COOLDOWN_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("[AdCooldown] Cooldowns saved.");
    }

    public void LoadCooldowns()
    {
        if (!PlayerPrefs.HasKey(COOLDOWN_KEY)) return;

        string json = PlayerPrefs.GetString(COOLDOWN_KEY);
        CooldownSaveData data = JsonUtility.FromJson<CooldownSaveData>(json);

        cooldowns.Clear();

        for (int i = 0; i < data.ids.Count; i++)
        {
            // If cooldown is still in the future, restore it
            if (data.endTimes[i] > Time.time)
                cooldowns[data.ids[i]] = data.endTimes[i];
        }

        Debug.Log("[AdCooldown] Cooldowns loaded.");
    }
}