using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
    public static GameConfigManager Instance { get; private set; }

    [Header("Global Config Reference")]
    public GameBalanceConfigSO config;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Enforce singleton
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public GameBalanceConfigSO Config => config;
}
