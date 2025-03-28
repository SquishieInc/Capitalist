using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Config/AdRewardCooldownConfig")]
public class AdRewardConfigSO : ScriptableObject
{
    [System.Serializable]
    public class RewardCooldown
    {
        public string rewardID; // "income_boost", etc.
        public float cooldownSeconds;
    }

    public List<RewardCooldown> rewards = new List<RewardCooldown>();
}