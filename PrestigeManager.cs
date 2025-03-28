using UnityEngine;
using System;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance;

    public double prestigePoints;
    public double unspentPrestigeCurrency;

    public TMPro.TMP_Text prestigeDisplay;
    public event Action OnPrestigeCurrencyChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public int CalculatePendingPrestige()
    {
        double totalEarned = CurrencyManager.Instance.GetTotalCashEarned();
        double divisor = GameConfigManager.Instance.Config.prestigeBaseDivisor;
        double rawPoints = Math.Pow(totalEarned / divisor, 0.5);
        return Mathf.FloorToInt((float)rawPoints);
    }

    public void PerformPrestigeReset()
    {
        int pointsGained = CalculatePendingPrestige();
        if (pointsGained <= 0)
        {
            Debug.LogWarning("Not enough progress for prestige.");
            return;
        }

        prestigePoints += pointsGained;
        unspentPrestigeCurrency += pointsGained;

        CurrencyManager.Instance.ResetCash();

        foreach (var p in FindObjectsOfType<MonoBehaviour>())
        {
            if (p is IPrestigeable prestigeable)
                prestigeable.OnPrestigeReset();
        }

        SaveSystem.Instance.SaveGame();
        OnPrestigeCurrencyChanged?.Invoke();
        UpdateDisplay();

        // ✅ NEW: Track with analytics
        AnalyticsManager.Instance.LogPrestige();

        // Optional: restart scene after reset
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void SpendPrestigeCurrency(double amount)
    {
        if (unspentPrestigeCurrency >= amount)
        {
            unspentPrestigeCurrency -= amount;
            OnPrestigeCurrencyChanged?.Invoke();
            UpdateDisplay();
        }
    }

    public void AddPrestigeCurrency(double amount)
    {
        unspentPrestigeCurrency += amount;
        OnPrestigeCurrencyChanged?.Invoke();
        UpdateDisplay();
    }

    public void LoadFromSave(double totalPoints, double unspentPoints)
    {
        prestigePoints = totalPoints;
        unspentPrestigeCurrency = unspentPoints;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (prestigeDisplay != null)
            prestigeDisplay.text = $"Prestige: {prestigePoints}  |  Unspent: {unspentPrestigeCurrency}";
    }
}
