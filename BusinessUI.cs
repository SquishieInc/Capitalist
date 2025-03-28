using UnityEngine;
using TMPro;

public class BusinessUI : MonoBehaviour
{
    [Header("References")]
    public BusinessController businessController;

    public TMP_Text businessNameText;
    public TMP_Text levelText;
    public TMP_Text costText;
    public TMP_Text incomeText;
    public TMP_Text milestoneBonusText;
    public TMP_Text nextMilestoneText;
    public TMP_Text managerStatusText;

    public void UpdateUI()
    {
        if (businessController == null || businessController.businessData == null) return;

        businessNameText.text = businessController.businessData.businessName;
        levelText.text = $"Lv. {businessController.level}";
        costText.text = $"Cost: ${businessController.GetCurrentCost():0}";
        incomeText.text = $"Income: ${businessController.GetIncomePerCycle():0}";

        float currentBonus = (float)businessController.GetLocalPrestigeMultiplier();
        milestoneBonusText.text = $"Milestone: x{currentBonus}";

        UpdateNextMilestonePreview();
        UpdateManagerStatus();
    }

    private void UpdateNextMilestonePreview()
    {
        var milestones = businessController.milestoneTable?.milestones;
        if (milestones == null || milestones.Count == 0)
        {
            nextMilestoneText.text = "";
            return;
        }

        float currentBonus = (float)businessController.GetLocalPrestigeMultiplier();
        string preview = "Maxed";

        foreach (var m in milestones)
        {
            if (m.requiredLevel > businessController.level && m.incomeMultiplier > currentBonus)
            {
                preview = $"Next x{m.incomeMultiplier} at Lv.{m.requiredLevel}";
                break;
            }
        }

        nextMilestoneText.text = preview;
    }

    private void UpdateManagerStatus()
    {
        if (businessController.managerUnlocked)
        {
            managerStatusText.text = "Manager: ✅ Hired";
        }
        else if (businessController.level >= GameConfigManager.Instance.Config.defaultManagerUnlockLevel)
        {
            managerStatusText.text = "Manager: ⏳ Unlockable";
        }
        else
        {
            managerStatusText.text = "Manager: 🔒 Locked";
        }
    }
}
