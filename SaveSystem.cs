using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public double cash;
        public int[] businessLevels;
        public bool[] managerStatuses;
    }

    public BusinessController[] businesses;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            cash = CurrencyManager.Instance.cash,
            businessLevels = new int[businesses.Length],
            managerStatuses = new bool[businesses.Length]
        };

        for (int i = 0; i < businesses.Length; i++)
        {
            data.businessLevels[i] = businesses[i].level;
            data.managerStatuses[i] = businesses[i].managerUnlocked;
        }

        File.WriteAllText(savePath, JsonUtility.ToJson(data));
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
        CurrencyManager.Instance.cash = data.cash;

        for (int i = 0; i < businesses.Length; i++)
        {
            businesses[i].level = data.businessLevels[i];
            businesses[i].managerUnlocked = data.managerStatuses[i];
        }
    }
}
