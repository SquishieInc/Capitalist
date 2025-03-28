using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HireManagerUI : MonoBehaviour
{
    public BusinessController controller;
    public GameObject hireButtonGroup;
    public TMP_Text costText;
    public Button hireButton;

    private void Start()
    {
        hireButton.onClick.AddListener(OnHireClicked);
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (controller == null) return;

        if (controller.managerUnlocked)
        {
            hireButtonGroup.SetActive(false);
            return;
        }

        double multiplier = GameConfigManager.Instance.Config.hireManagerCostMultiplier;
        double hireCost = controller.GetIncomePerCycle() * multiplier;
        costText.text = $"Hire Manager\nCost: ${hireCost:0}";
        hireButtonGroup.SetActive(true);
    }

    private void OnHireClicked()
    {
        double multiplier = GameConfigManager.Instance.Config.hireManagerCostMultiplier;
        double hireCost = controller.GetIncomePerCycle() * multiplier;

        if (CurrencyManager.Instance.SpendCash(hireCost))
        {
            controller.HireManager();
            hireButtonGroup.SetActive(false);
        }
    }
}
