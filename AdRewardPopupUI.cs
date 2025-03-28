using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdRewardPopupUI : MonoBehaviour
{
    public Button incomeButton;
    public Button speedButton;
    public Button autoCollectButton;

    public TMP_Text incomeTimer;
    public TMP_Text speedTimer;
    public TMP_Text autoTimer;

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdateCooldowns), 0f, 1f);

        incomeButton.onClick.AddListener(() => AdManager.Instance.ShowRewardedAd("income_boost"));
        speedButton.onClick.AddListener(() => AdManager.Instance.ShowRewardedAd("speed_boost"));
        autoCollectButton.onClick.AddListener(() => AdManager.Instance.ShowRewardedAd("autocollect"));
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateCooldowns));
    }

    private void UpdateCooldowns()
    {
        UpdateButton("income_boost", incomeButton, incomeTimer);
        UpdateButton("speed_boost", speedButton, speedTimer);
        UpdateButton("autocollect", autoCollectButton, autoTimer);
    }

    private void UpdateButton(string id, Button btn, TMP_Text timerText)
    {
        if (AdCooldownManager.Instance.IsOnCooldown(id))
        {
            float remaining = AdCooldownManager.Instance.GetRemainingCooldown(id);
            timerText.text = $"Wait: {Mathf.CeilToInt(remaining)}s";
            btn.interactable = false;
        }
        else
        {
            timerText.text = "Ready!";
            btn.interactable = true;
        }
    }
}