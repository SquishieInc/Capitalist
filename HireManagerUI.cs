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
        UpdateUI(); // Refresh live if income changes or manager is hired
    }

    private void UpdateUI()
    {
        if (controller == null) return;

        if (controller.managerUnlocked)
        {
            hireButtonGroup.SetActive(false);
            return;
        }

        double hireCost = controller.GetIncomePerCycle() * 5;
        costText.text = $"Hire Manager\nCost: ${hireCost:0}";

        hireButtonGroup.SetActive(true);
    }

    private void OnHireClicked()
    {
        double hireCost = controller.GetIncomePerCycle() * 5;

        if (CurrencyManager.Instance.SpendCash(hireCost))
        {
            controller.HireManager();
            hireButtonGroup.SetActive(false);
        }
    }
}
