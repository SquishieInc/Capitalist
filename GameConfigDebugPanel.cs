using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameConfigDebugPanel : MonoBehaviour
{
    [Header("References")]
    public GameBalanceConfigSO config;

    [Header("UI Fields")]
    public TMP_InputField prestigeDivisorInput;
    public TMP_InputField offlineRateInput;
    public TMP_InputField cheatPenaltyInput;

    public TMP_InputField hireManagerMultiplierInput;
    public TMP_InputField costMultiplierInput;

    public Button applyButton;
    public TMP_Text statusText;

    private void Start()
    {
        LoadConfigValues();
        applyButton.onClick.AddListener(ApplyChanges);
    }

    private void LoadConfigValues()
    {
        prestigeDivisorInput.text = config.prestigeBaseDivisor.ToString();
        offlineRateInput.text = config.baseOfflineEarningsRate.ToString();
        cheatPenaltyInput.text = config.timeCheatPenaltyMultiplier.ToString();

        hireManagerMultiplierInput.text = config.hireManagerCostMultiplier.ToString();
        costMultiplierInput.text = config.defaultCostMultiplier.ToString();
    }

    private void ApplyChanges()
    {
        if (double.TryParse(prestigeDivisorInput.text, out double pd))
            config.prestigeBaseDivisor = pd;

        if (float.TryParse(offlineRateInput.text, out float or))
            config.baseOfflineEarningsRate = or;

        if (float.TryParse(cheatPenaltyInput.text, out float cp))
            config.timeCheatPenaltyMultiplier = cp;

        if (float.TryParse(hireManagerMultiplierInput.text, out float hm))
            config.hireManagerCostMultiplier = hm;

        if (float.TryParse(costMultiplierInput.text, out float cm))
            config.defaultCostMultiplier = cm;

        statusText.text = "✅ Config updated at runtime!";
        Debug.Log("[Debug Panel] Config updated!");
    }
}
