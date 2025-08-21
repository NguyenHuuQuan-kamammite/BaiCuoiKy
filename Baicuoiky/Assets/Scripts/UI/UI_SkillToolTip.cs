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
        skillTree = ui.GetComponentInChildren<UI_SkillTree>();
    }
    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);

    }

    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);
        if (node == null || node.skillData == null)
        {
            Debug.LogWarning("Node or skill data is null.");
            return;
        }
        skillName.text = node.skillData.skillName;
        skillDescription.text = node.skillData.description;
        string skillLockText = $"<color={importantInfoHex}>{lockSkillText}</color>";
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
        sb.AppendLine($"<color={costColor}>- {skillCost} Skill Points</color>");
        foreach (var node in neededNodes)
        {
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName}</color>");
        }
        if (conflictNodes.Length <= 0)
            return sb.ToString();


        sb.AppendLine();
        sb.AppendLine($"<color={importantInfoHex}>Locks out:</color>");
        foreach (var node in conflictNodes)
        {

            sb.AppendLine($"<color={importantInfoHex}>- {node.skillData.skillName}</color>");
        }
        return sb.ToString();

    }
    private string GetColorText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
   
}