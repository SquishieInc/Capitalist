using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public int saveVersion = 1;

        public double cash;
        public double totalCashEarned;
        public int gems;

        public int[] businessLevels;
        public bool[] managerStatuses;

        public double prestigePoints;
        public double unspentPrestigeCurrency;

        public int[] prestigeUpgradeLevels;
    }

    [Header("Game References")]
    public BusinessController[] businesses;
    public PrestigeShopManager prestigeShopManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SaveGame()
    {
        SaveData data = GetCurrentSaveData();
        File.WriteAllText(savePath, JsonUtility.ToJson(data));
        Debug.Log("[SaveSystem] Game saved.");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("[SaveSystem] No save file found.");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
        SaveMigrationManager.Migrate(ref data);

        CurrencyManager.Instance.cash = data.cash;
        CurrencyManager.Instance.totalCashEarned = data.totalCashEarned;
        CurrencyManager.Instance.gems = data.gems;

        for (int i = 0; i < businesses.Length; i++)
        {
            businesses[i].level = data.businessLevels[i];
            businesses[i].managerUnlocked = data.managerStatuses[i];
        }

        PrestigeManager.Instance.LoadFromSave(data.prestigePoints, data.unspentPrestigeCurrency);

        for (int i = 0; i < prestigeShopManager.upgrades.Count; i++)
        {
            var upgrade = prestigeShopManager.upgrades[i];
            prestigeShopManager.SetUpgradeLevel(upgrade, data.prestigeUpgradeLevels[i]);
        }

        Debug.Log("[SaveSystem] Game loaded.");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("[SaveSystem] Save file deleted.");
        }
    }

    public SaveData GetCurrentSaveData()
    {
        SaveData data = new SaveData
        {
            saveVersion = SaveMigrationManager.CURRENT_VERSION,
            cash = CurrencyManager.Instance.cash,
            totalCashEarned = CurrencyManager.Instance.totalCashEarned,
            gems = CurrencyManager.Instance.gems,

            businessLevels = new int[businesses.Length],
            managerStatuses = new bool[businesses.Length],
            prestigePoints = PrestigeManager.Instance.prestigePoints,
            unspentPrestigeCurrency = PrestigeManager.Instance.unspentPrestigeCurrency,
            prestigeUpgradeLevels = new int[prestigeShopManager.upgrades.Count]
        };

        for (int i = 0; i < businesses.Length; i++)
        {
            data.businessLevels[i] = businesses[i].level;
            data.managerStatuses[i] = businesses[i].managerUnlocked;
        }

        for (int i = 0; i < prestigeShopManager.upgrades.Count; i++)
        {
            var upgrade = prestigeShopManager.upgrades[i];
            data.prestigeUpgradeLevels[i] = prestigeShopManager.GetUpgradeLevel(upgrade);
        }

        return data;
    }
}