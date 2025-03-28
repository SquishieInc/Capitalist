[CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Idle/Game Balance")]
public class GameBalanceConfig : ScriptableObject
{
    public float prestigeMultiplierPerPoint = 0.1f;
    public float hireManagerCostMultiplier = 5.0f;
    public float offlineEarningsPenalty = 0.25f;
    public int[] managerUnlockLevels = new int[] { 25, 50, 100 };
}
