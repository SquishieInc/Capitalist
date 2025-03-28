using System.Collections.Generic;
using UnityEngine;

public enum MilestoneEffectType
{
    IncomeMultiplier,
    AutoCollect,
    GemsReward,
    SpeedBoost,
    PlayEffect
}

[CreateAssetMenu(fileName = "BusinessMilestone", menuName = "Idle/Business Milestone Table")]
public class BusinessMilestoneSO : ScriptableObject
{
    [System.Serializable]
    public class Milestone
    {
        public int requiredLevel;
        public float incomeMultiplier = 1f;

        public MilestoneEffectType effectType = MilestoneEffectType.IncomeMultiplier;
        public int effectValue = 0; // Used for gem amount, speed %, etc.
    }

    public List<Milestone> milestones = new List<Milestone>();
}