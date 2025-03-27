using UnityEngine;

[CreateAssetMenu(fileName = "NewBusiness", menuName = "Idle/Business")]
public class BusinessSO : ScriptableObject
{
    public string businessName;
    public double baseCost;
    public double baseIncome;
    public float incomeInterval = 1f;
}
