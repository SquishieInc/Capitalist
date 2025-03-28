using UnityEngine;

[CreateAssetMenu(fileName = "NewBusiness", menuName = "Idle/Business")]
public class BusinessSO : ScriptableObject
{
    [Header("Basic Info")]
    public string businessName;
    public double baseCost;
    public double baseIncome;
    public float incomeInterval = 1f;

    [Header("Scaling")]
    public float costMultiplier = 1.15f;      // Cost increase per level
    public float incomeGrowth = 1.0f;         // Exponential income scaling
}
