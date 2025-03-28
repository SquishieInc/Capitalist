using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBusinessMilestones", menuName = "Idle/Business Milestone Table")]
public class BusinessMilestoneSO : ScriptableObject
{

    public enum MilestoneEffectType
    {
        IncomeMultiplier,
        AutoCollect,
        GemsReward,
        SpeedBoost,
        PlayEffect
    }

    [System.Serializable]
    public class Milestone
    {
        public int requiredLevel;
        public float incomeMultiplier = 1.0f;
    }

    public List<Milestone> milestones = new List<Milestone>();
}
