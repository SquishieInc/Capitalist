using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUI : MonoBehaviour
{
    public BusinessController controller;
    public TMP_Text businessName;
    public TMP_Text levelText;
    public TMP_Text costText;
    public TMP_Text incomeText;
    public Button levelUpButton;

    private void Start()
    {
        levelUpButton.onClick.AddListener(() =>
        {
            controller.LevelUp();
            UpdateUI();
        });

        UpdateUI();
    }

    private void UpdateUI()
    {
        businessName.text = controller.businessData.businessName;
        levelText.text = $"Level: {controller.level}";
        costText.text = $"Upgrade: ${controller.GetCurrentCost():0}";
        incomeText.text = $"Income: ${controller.GetIncomePerCycle():0}/sec";
    }
}
