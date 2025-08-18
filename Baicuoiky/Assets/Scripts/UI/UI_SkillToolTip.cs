using UnityEngine;
using TMPro;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);

    }

    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSo skillData)
    {
        base.ShowToolTip(show, targetRect);
        if (show == false)
        {
            return;
        }
        skillName.text = skillData.skillName;
        skillDescription.text = skillData.description;
        skillRequirements.text = "Requirements: \n" + " -" + skillData.cost + " " + "Skill Points";
    }
}