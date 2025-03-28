using UnityEngine;
using UnityEngine.UI;

public class DevCheatPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button addCashButton;
    public Button addGemsButton;
    public Button prestigeButton;
    public Button levelUpAllButton;
    public Button unlockManagersButton;
    public Button simulatePurchaseButton;
    public Button logSaveButton;
    public Button clearSaveButton;

    private void Start()
    {
        addCashButton.onClick.AddListener(() => CurrencyManager.Instance.AddCash(1000));
        addGemsButton.onClick.AddListener(() => CurrencyManager.Instance.AddGems(10));
        prestigeButton.onClick.AddListener(() => PrestigeManager.Instance.PerformPrestigeReset());

        levelUpAllButton.onClick.AddListener(LevelUpAllBusinesses);
        unlockManagersButton.onClick.AddListener(UnlockAllManagers);
        simulatePurchaseButton.onClick.AddListener(SimulatePurchase);
        logSaveButton.onClick.AddListener(LogSaveData);
        clearSaveButton.onClick.AddListener(() => SaveSystem.Instance.DeleteSave());
    }

    private void LevelUpAllBusinesses()
    {
        foreach (var b in SaveSystem.Instance.businesses)
        {
            for (int i = 0; i < 10; i++) b.LevelUp();
        }
        Debug.Log("[DevCheat] All businesses leveled up by 10.");
    }

    private void UnlockAllManagers()
    {
        foreach (var b in SaveSystem.Instance.businesses)
        {
            b.HireManager();
        }
        Debug.Log("[DevCheat] All managers unlocked.");
    }

    private void SimulatePurchase()
    {
        CurrencyManager.Instance.AddGems(100);
        Debug.Log("[DevCheat] Simulated IAP - Added 100 gems.");
    }

    private void LogSaveData()
    {
        string json = JsonUtility.ToJson(SaveSystem.Instance, true);
        Debug.Log("[DevCheat] Current Save JSON:\n" + json);
    }
}