using UnityEngine;

public class BusinessController : MonoBehaviour
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
            double income = businessData.baseIncome * level;
            CurrencyManager.Instance.AddCash(income);
        }
    }

    public double GetCurrentCost() => currentCost;
    public double GetIncomePerCycle() => businessData.baseIncome * level;
}
