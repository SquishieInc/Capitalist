using UnityEngine;
using UnityEngine.UI;

public class PrestigeDebugButton : MonoBehaviour
{
    public Button prestigeButton;

    private void Start()
    {
        if (prestigeButton != null)
        {
            prestigeButton.onClick.AddListener(() =>
            {
                PrestigeManager.Instance.PerformPrestigeReset();
            });
        }
    }
}
