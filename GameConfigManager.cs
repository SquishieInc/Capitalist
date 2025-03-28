using UnityEngine;
using System;

public class GameConfigManager : MonoBehaviour
{
    public static GameConfigManager Instance { get; private set; }

    [Header("Global Config Reference")]
    public GameBalanceConfigSO config;

    public event Action OnConfigUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameBalanceConfigSO Config
    {
        get => config;
        set
        {
            config = value;
            NotifyConfigUpdated();
        }
    }

    public void NotifyConfigUpdated()
    {
        OnConfigUpdated?.Invoke();
        Debug.Log("[GameConfigManager] Config updated & sync triggered.");
    }
}
