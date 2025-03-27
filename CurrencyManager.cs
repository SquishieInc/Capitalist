using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public double cash;
    public double gems;
    public double eventCurrency;

    public event Action OnCurrencyUpdated;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddCash(double amount)
    {
        cash += amount;
        OnCurrencyUpdated?.Invoke();
    }

    public void AddGems(double amount)
    {
        gems += amount;
        OnCurrencyUpdated?.Invoke();
    }

    public void AddEventCurrency(double amount)
    {
        eventCurrency += amount;
        OnCurrencyUpdated?.Invoke();
    }

    public bool SpendCash(double amount)
    {
        if (cash >= amount)
        {
            cash -= amount;
            OnCurrencyUpdated?.Invoke();
            return true;
        }
        return false;
    }
}
