using UnityEngine;
using Unity.RemoteConfig;

public class RemoteEventController : MonoBehaviour
{
    public static RemoteEventController Instance;

    public bool eventActive;
    public string eventName;
    public double eventMultiplier;
    public string eventCurrency;
    public int eventDurationHours;

    struct userAttributes { }
    struct appAttributes { }

    private void Awake() => Instance = this;

    private void Start()
    {
        ConfigManager.FetchCompleted += ApplyRemoteSettings;
        ConfigManager.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void ApplyRemoteSettings(ConfigResponse response)
    {
        eventActive = ConfigManager.appConfig.GetBool("event_active");
        eventName = ConfigManager.appConfig.GetString("event_name");
        eventMultiplier = ConfigManager.appConfig.GetFloat("event_multiplier");
        eventCurrency = ConfigManager.appConfig.GetString("event_currency");
        eventDurationHours = ConfigManager.appConfig.GetInt("event_duration");
    }

    public double GetEventMultiplier() => eventActive ? eventMultiplier : 1.0;
}
