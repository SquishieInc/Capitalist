using UnityEngine;
using UnityEngine.UI;

public class RateAppManager : MonoBehaviour
{
    public static RateAppManager Instance;

    public GameObject popupPanel;
    public Button rateButton;
    public Button closeButton;

    private int sessionsBeforePrompt = 3;
    private string rateKey = "hasRated";

    private void Awake()
    {
        if (Instance == null) Instance = this;

        popupPanel.SetActive(false);
        rateButton.onClick.AddListener(OnRate);
        closeButton.onClick.AddListener(() => popupPanel.SetActive(false));
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(rateKey, 0) == 1)
            return;

        int prestigeCount = PlayerPrefs.GetInt("prestige_count", 0);
        if (prestigeCount >= sessionsBeforePrompt)
        {
            popupPanel.SetActive(true);
        }
    }

    public void RegisterPrestige()
    {
        int current = PlayerPrefs.GetInt("prestige_count", 0);
        PlayerPrefs.SetInt("prestige_count", current + 1);
    }

    private void OnRate()
    {
        PlayerPrefs.SetInt(rateKey, 1);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.yourcompany.yourgame");
#elif UNITY_IOS
        Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_APP_ID");
#else
        Application.OpenURL("https://yourgame.com");
#endif
        popupPanel.SetActive(false);
    }
}