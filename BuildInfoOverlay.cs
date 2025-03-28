using UnityEngine;
using TMPro;

public class BuildInfoOverlay : MonoBehaviour
{
    public TMP_Text infoText;

    private void Start()
    {
        string version = Application.version;
        string platform = Application.platform.ToString();
        string buildDate = GetBuildDate();

        infoText.text = $"v{version} | {platform} | {buildDate}";
    }

    private string GetBuildDate()
    {
        System.DateTime now = System.DateTime.Now;
        return now.ToString("yyyy-MM-dd HH:mm");
    }
}