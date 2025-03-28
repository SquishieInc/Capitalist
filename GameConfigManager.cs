using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NotifyConfigUpdated(); // Resync config to all listeners
        Debug.Log($"[GameConfigManager] Config re-synced on scene load: {scene.name}");
    }
}
