using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance;

    [Header("Prestige Config")]
    public double baseDivisor = 1_000_000; // Cash required per 1 Prestige Point
    public double prestigePoints;
    public double unspentPrestigeCurrency;

    [Header("UI Display (Optional)")]
    public TMPro.TMP_Text prestigeDisplay;

    // Hook to notify UI/shop of update
    public event Action OnPrestigeCurrencyChanged;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
{
    LoadUpgradeProgress();
    RenderUpgradeList();
}


    /// <summary>
    /// Calculates how many prestige points the player would earn on reset.
    /// </summary>
    public int CalculatePendingPrestige()
    {
        double totalEarned = CurrencyManager.Instance.GetTotalCashEarned();
        double rawPoints = Math.Pow(totalEarned / baseDivisor, 0.5);
        return Mathf.FloorToInt((float)rawPoints);
    }

    /// <summary>
    /// Triggers a prestige reset: adds currency and resets relevant systems.
    /// </summary>
    public void PerformPrestigeReset()
    {
        int pointsGained = CalculatePendingPrestige();
        if (pointsGained <= 0)
        {
            Debug.LogWarning("Not enough progress for prestige.");
            return;
        }

        // Add prestige points
        prestigePoints += pointsGained;
        unspentPrestigeCurrency += pointsGained;

        // Reset currencies
        CurrencyManager.Instance.ResetCash();

        // Reset all prestigeable systems
        foreach (var p in FindObjectsOfType<MonoBehaviour>())
        {
            if (p is IPrestigeable prestigeable)
                prestigeable.OnPrestigeReset();
        }

        SaveSystem.Instance.SaveGame();
        Debug.Log($"[Prestige] +{pointsGained} points earned. Total: {prestigePoints}");
        OnPrestigeCurrencyChanged?.Invoke();
        UpdateDisplay();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    private void SaveUpgradeProgress()
{
    foreach (var kv in upgradeLevels)
    {
        string key = $"prestige_upgrade_{kv.Key.upgradeName}";
        PlayerPrefs.SetInt(key, kv.Value);
    }
    PlayerPrefs.Save();
}

private void LoadUpgradeProgress()
{
    upgradeLevels.Clear();

    foreach (var upgrade in upgrades)
    {
        string key = $"prestige_upgrade_{upgrade.upgradeName}";
        int level = PlayerPrefs.GetInt(key, 0);
        upgradeLevels[upgrade] = level;
    }
}

}
