using UnityEngine;

public class DevToolsManager : MonoBehaviour
{
    public static DevToolsManager Instance;

    [Header("Tool Toggles")]
    public GameObject analyticsOverlay;
    public GameObject configDebugPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Hide if not a debug build
        if (!Debug.isDebugBuild)
        {
            analyticsOverlay.SetActive(false);
            configDebugPanel.SetActive(false);
            return;
        }

        analyticsOverlay.SetActive(true);
        configDebugPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            configDebugPanel.SetActive(!configDebugPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            analyticsOverlay.SetActive(!analyticsOverlay.activeSelf);
        }
    }
}
