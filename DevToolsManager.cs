using UnityEngine;

public class DevToolsManager : MonoBehaviour
{
    public static DevToolsManager Instance;

    [Header("Tool Toggles")]
    public GameObject configDebugPanel;
    public GameObject analyticsOverlay;
    public GameObject cheatPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // Hide everything if not in a debug/development build
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
        if (configDebugPanel) configDebugPanel.SetActive(false);
        if (analyticsOverlay) analyticsOverlay.SetActive(false);
        if (cheatPanel) cheatPanel.SetActive(false);
        enabled = false;
#else
        if (configDebugPanel) configDebugPanel.SetActive(false);
        if (analyticsOverlay) analyticsOverlay.SetActive(false);
        if (cheatPanel) cheatPanel.SetActive(false);
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Input.GetKeyDown(KeyCode.F1) && configDebugPanel != null)
        {
            configDebugPanel.SetActive(!configDebugPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F2) && analyticsOverlay != null)
        {
            analyticsOverlay.SetActive(!analyticsOverlay.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F3) && cheatPanel != null)
        {
            cheatPanel.SetActive(!cheatPanel.activeSelf);
        }
#endif
    }
}