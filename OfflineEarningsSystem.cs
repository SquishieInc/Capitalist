using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class OfflineEarningsSystem : MonoBehaviour
{
    private const string LastExitKey = "LastExitTime";
    private DateTime serverTime, localExitTime;
    private double calculatedEarnings;

    public OfflineRewardedAdPopup adPopup;

    private void Start()
    {
        StartCoroutine(FetchServerTime((success) =>
        {
            if (success)
            {
                ProcessOfflineEarnings();
                SyncEventTimer();
            }
        }));
    }

    private IEnumerator FetchServerTime(Action<bool> callback)
    {
        string url = "http://worldtimeapi.org/api/timezone/Etc/UTC";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string datetime = www.downloadHandler.text.Split(new string[] { "\"datetime\":\"" }, StringSplitOptions.None)[1].Split('\"')[0];
            serverTime = DateTime.Parse(datetime);
        }
        else
        {
            serverTime = DateTime.UtcNow;
        }

        callback.Invoke(true);
    }

    private void ProcessOfflineEarnings()
    {
        if (!PlayerPrefs.HasKey(LastExitKey)) return;

        localExitTime = DateTime.Parse(PlayerPrefs.GetString(LastExitKey));
        TimeSpan offlineDuration = serverTime - localExitTime;
        if (offlineDuration.TotalSeconds <= 0) return;

        AntiCheatManager.Instance.RunAntiCheatCheck(serverTime);

        double baseRate = GameConfigManager.Instance.Config.baseOfflineEarningsRate;
        double baseEarnings = offlineDuration.TotalSeconds * baseRate;

        float offlineBoost = PrestigeShopManager.Instance.GetTotalEffect(PrestigeUpgradeSO.UpgradeType.OfflineEarningsBoost);
        double multiplier = 1.0 + offlineBoost;

        calculatedEarnings = AntiCheatManager.Instance.ApplyPenaltyIfCheating(baseEarnings * multiplier, true);

        adPopup.ShowPopup(calculatedEarnings);
    }

    public void GrantOfflineEarnings(bool doubled)
    {
        double reward = doubled ? calculatedEarnings * 2 : calculatedEarnings;
        CurrencyManager.Instance.AddCash(reward);
        Debug.Log($"Offline earnings granted: ${reward}");
    }

    private void SyncEventTimer() { }

    private void OnApplicationQuit() => SaveExitTime();
    private void OnApplicationPause(bool pause) { if (pause) SaveExitTime(); }

    private void SaveExitTime()
    {
        PlayerPrefs.SetString(LastExitKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }
}
