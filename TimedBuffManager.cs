using UnityEngine;
using System.Collections.Generic;

public class TimedBuffManager : MonoBehaviour
{
    public static TimedBuffManager Instance;

    private class ActiveBuff
    {
        public TimedBuffType type;
        public float value;
        public float remainingTime;
    }

    private readonly List<ActiveBuff> activeBuffs = new List<ActiveBuff>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].remainingTime -= Time.deltaTime;
            if (activeBuffs[i].remainingTime <= 0)
                activeBuffs.RemoveAt(i);
        }
    }

    public void ApplyBuff(TimedBuffType type, float value, float duration)
    {
        activeBuffs.Add(new ActiveBuff
        {
            type = type,
            value = value,
            remainingTime = duration
        });

        Debug.Log($"[Buff] Applied {type} x{value} for {duration} sec");
    }

    public float GetBuffValue(TimedBuffType type)
    {
        float total = 0f;

        foreach (var buff in activeBuffs)
        {
            if (buff.type == type)
                total += buff.value;
        }

        return total;
    }

    public bool IsBuffActive(TimedBuffType type)
    {
        return GetBuffValue(type) > 0f;
    }
}