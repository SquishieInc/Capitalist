using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text businessName;
    public TMP_Text levelText;
    public TMP_Text costText;
    public TMP_Text incomeText;
    public TMP_Text milestoneBonusText;
    public Button levelUpButton;

    [Header("Business Logic")]
    public BusinessController controller;

    private void Start()
    {
        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(OnLevelUpClicked);

        UpdateUI();
    }

    private void Update()
    {
        // Optionally auto-refresh milestone text live
        UpdateUI(); 
    }

    private void OnLevelUpClicked()
    {
        controller.LevelUp();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (controller == null || controller.businessData == null) return;

        businessName.text = controller.businessData.businessName;
        levelText.text = $"Level: {controller.level}";
        costText.text = $"Upgrade: ${controller.GetCurrentCost():0}";
        incomeText.text = $"Income: ${controller.GetIncomePerCycle():0}/sec";

        double milestoneBonus = controller.GetLocalMilestoneMultiplier();
        milestoneBonusText.text = $"Milestone Bonus: x{milestoneBonus:0.0}";
    }
}
