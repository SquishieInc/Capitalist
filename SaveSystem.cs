using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private string savePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    public class SaveData
    {
        public double cash;
        public int[] businessLevels;
    }

    public BusinessController[] businesses;

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            cash = CurrencyManager.Instance.cash,
            businessLevels = new int[businesses.Length]
        };

        for (int i = 0; i < businesses.Length; i++)
        {
            data.businessLevels[i] = businesses[i].level;
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
        }
    }
}
