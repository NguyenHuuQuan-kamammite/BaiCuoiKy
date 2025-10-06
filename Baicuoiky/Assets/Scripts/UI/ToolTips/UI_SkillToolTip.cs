using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCoolDown;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string importantInfoHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockSkillText = "You're taking a different path - this skill is now locked.";

    private Coroutine textEffectCo;
    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }
    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);

    }

    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSo skillData, UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);
       if(show==false)
            return;
        skillName.text = skillData.skillName;
        skillDescription.text = skillData.description;
        skillCoolDown.text = "Cooldown:"+ skillData.upgradeData.cooldown + "s.";


        if(node == null)
        {
            skillRequirements.text = "";
            return;
        }

        string skillLockText = GetColorText(importantInfoHex, lockSkillText);
        string requirements = node.isLocked ? skillLockText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);
        skillRequirements.text = requirements;

    }

    public void LockedSkillEffect()
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }
        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements, 0.15f, 3));
    }
     private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)

    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColorText(notMetConditionHex, lockSkillText);
            yield return new WaitForSeconds(blinkInterval);
            text.text = GetColorText(importantInfoHex, lockSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirement");
        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metConditionHex : notMetConditionHex;
        string costText = $"-{skillCost} Skill Points";
        string finalCostText = GetColorText(costColor, costText);
        sb.AppendLine(finalCostText);
        foreach (var node in neededNodes)
        {
            if(node == null) continue;
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColorText(nodeColor, nodeText);
            sb.AppendLine(finalNodeText);
        }
        if (conflictNodes.Length <= 0)
            return sb.ToString();


        sb.AppendLine();
        sb.AppendLine(GetColorText(importantInfoHex, "Locks out:"));
        foreach (var node in conflictNodes)
        {
            if(node == null) continue;
            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColorText(importantInfoHex, nodeText);

            sb.AppendLine(finalNodeText);
        }
        return sb.ToString();

    }
  
   
}