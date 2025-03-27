using UnityEngine;

public class BusinessController : MonoBehaviour, IPrestigeable
{
    public BusinessSO businessData;
    public int level = 0;
    private double currentCost;

    private void Start()
    {
        currentCost = businessData.baseCost;
        InvokeRepeating(nameof(GenerateIncome), businessData.incomeInterval, businessData.incomeInterval);
    }

    public void LevelUp()
    {
        if (CurrencyManager.Instance.SpendCash(currentCost))
        {
            level++;
            currentCost *= 1.15;
        }
    }

    private void GenerateIncome()
    {
        if (level > 0)
        {
            double baseIncome = businessData.baseIncome * level;
            double multiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints); // +10% per point
            double finalIncome = baseIncome * multiplier;

            CurrencyManager.Instance.AddCash(finalIncome);
        }
    }

    public double GetCurrentCost() => currentCost;
    public double GetIncomePerCycle()
    {
        double baseIncome = businessData.baseIncome * level;
        double multiplier = 1.0 + (0.1 * PrestigeManager.Instance.prestigePoints);
        return baseIncome * multiplier;
    }

    // 🧼 Reset logic for PrestigeManager
    public void OnPrestigeReset()
    {
        level = 0;
        currentCost = businessData.baseCost;
    }
}
