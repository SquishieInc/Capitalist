using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Primary Currencies")]
    public double cash = 0;
    public double totalCashEarned = 0;

    [Header("Premium Currency")]
    public int gems = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddCash(double amount)
    {
        cash += amount;
        totalCashEarned += amount;

        AnalyticsManager.Instance.LogEvent("cash_earned", $"amount={amount}");
    }

    public bool SpendCash(double amount)
    {
        if (cash >= amount)
        {
            cash -= amount;
            AnalyticsManager.Instance.LogEvent("cash_spent", $"amount={amount}");
            return true;
        }
        return false;
    }

    public void AddGems(int amount)
    {
        gems += amount;
        SaveSystem.Instance.SaveGame(); // ✅ Save on gain
        AnalyticsManager.Instance.LogEvent("gems_earned", $"amount={amount}");
    }

    public bool SpendGems(int amount)
    {
        if (gems >= amount)
        {
            gems -= amount;
            SaveSystem.Instance.SaveGame(); // ✅ Save on spend
            AnalyticsManager.Instance.LogEvent("gems_spent", $"amount={amount}");
            return true;
        }
        return false;
    }

    public double GetTotalCashEarned() => totalCashEarned;

    public void ResetCash()
    {
        cash = 0;
        totalCashEarned = 0;
    }
}
