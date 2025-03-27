using UnityEngine;
using TMPro;

public class OfflineRewardedAdPopup : MonoBehaviour
{
    public GameObject popupUI;
    public TMP_Text earningsText;

    private double earnings;
    private OfflineEarningsSystem earningsSystem;

    private void Start() => earningsSystem = FindObjectOfType<OfflineEarningsSystem>();

    public void ShowPopup(double earned)
    {
        earnings = earned;
        earningsText.text = $"You earned ${earned:0} while offline!\nWatch an ad to DOUBLE it?";
        popupUI.SetActive(true);
    }

    public void WatchAdAndDouble()
    {
        AdManager.Instance.ShowRewardedAd(); // Reward is triggered via AdManager
        earningsSystem.GrantOfflineEarnings(true);
        popupUI.SetActive(false);
    }

    public void ClaimBaseEarnings()
    {
        earningsSystem.GrantOfflineEarnings(false);
        popupUI.SetActive(false);
    }
}
