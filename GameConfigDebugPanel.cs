using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameConfigDebugPanel : MonoBehaviour
{
    [Header("References")]
    public GameObject panelRoot;
    public GameBalanceConfigSO config;

    [Header("UI Fields")]
    public TMP_InputField prestigeDivisorInput;
    public TMP_InputField offlineRateInput;
    public TMP_InputField cheatPenaltyInput;
    public TMP_InputField hireManagerMultiplierInput;
    public TMP_InputField costMultiplierInput;

    public Button applyButton;
    public TMP_Text statusText;

    [Header("Config Presets")]
    public GameBalanceConfigSO[] profilePresets;
    public TMP_Dropdown profileDropdown;

    private void Start()
    {
        LoadConfigValues();
        BuildProfileDropdown();

        applyButton.onClick.AddListener(ApplyChanges);
        profileDropdown.onValueChanged.AddListener(LoadProfileByIndex);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            panelRoot.SetActive(!panelRoot.activeSelf);
        }
#endif
    }

    private void LoadConfigValues()
    {
        if (GameConfigManager.Instance.Config == null) return;

        config = GameConfigManager.Instance.Config;

        prestigeDivisorInput.text = config.prestigeBaseDivisor.ToString();
        offlineRateInput.text = config.baseOfflineEarningsRate.ToString();
        cheatPenaltyInput.text = config.timeCheatPenaltyMultiplier.ToString();
        hireManagerMultiplierInput.text = config.hireManagerCostMultiplier.ToString();
        costMultiplierInput.text = config.defaultCostMultiplier.ToString();
    }

    private void ApplyChanges()
    {
        if (config == null) return;

        double.TryParse(prestigeDivisorInput.text, out config.prestigeBaseDivisor);
        float.TryParse(offlineRateInput.text, out config.baseOfflineEarningsRate);
        float.TryParse(cheatPenaltyInput.text, out config.timeCheatPenaltyMultiplier);
        float.TryParse(hireManagerMultiplierInput.text, out config.hireManagerCostMultiplier);
        float.TryParse(costMultiplierInput.text, out config.defaultCostMultiplier);

        GameConfigManager.Instance.NotifyConfigUpdated();
        statusText.text = "✅ Config applied!";
    }

    private void BuildProfileDropdown()
    {
        profileDropdown.ClearOptions();
        List<string> names = new List<string>();
        foreach (var preset in profilePresets)
        {
            names.Add(preset.name);
        }
        profileDropdown.AddOptions(names);
    }

    private void LoadProfileByIndex(int index)
    {
        if (index >= 0 && index < profilePresets.Length)
        {
            GameConfigManager.Instance.Config = profilePresets[index];
            LoadConfigValues();
        }
    }
}
