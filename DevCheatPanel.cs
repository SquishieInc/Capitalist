using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    public Button exportSaveButton;

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
        exportSaveButton.onClick.AddListener(ExportSaveToFile);
    }

    private void LevelUpAllBusinesses()
    {
        foreach (var b in SaveSystem.Instance.businesses)
        {
            for (int i = 0; i < 10; i++)
                b.LevelUp();
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
        var data = SaveSystem.Instance.GetCurrentSaveData();
        string json = JsonUtility.ToJson(data, true);
        Debug.Log("[DevCheat] Save JSON:\n" + json);
    }

    private void ExportSaveToFile()
    {
        var data = SaveSystem.Instance.GetCurrentSaveData();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, "SaveExport.json");

        File.WriteAllText(path, json);
        Debug.Log($"[DevCheat] Save exported to file: {path}");
    }
}