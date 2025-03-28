using UnityEngine;
using System.Collections.Generic;

public class AdCooldownManager : MonoBehaviour
{
    public static AdCooldownManager Instance;

    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private float defaultCooldownDuration = 300f; // 5 minutes

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public bool IsOnCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return false;
        return Time.time < cooldowns[adType];
    }

    public float GetRemainingCooldown(string adType)
    {
        if (!cooldowns.ContainsKey(adType)) return 0f;
        return Mathf.Max(0, cooldowns[adType] - Time.time);
    }

    public void StartCooldown(string adType, float duration = -1f)
    {
        float cooldown = (duration > 0f) ? duration : defaultCooldownDuration;
        cooldowns[adType] = Time.time + cooldown;
        Debug.Log($"[AdCooldown] Started cooldown for '{adType}' ({cooldown} sec)");
    }
}