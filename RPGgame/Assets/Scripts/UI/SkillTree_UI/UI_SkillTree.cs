using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour,ISaveable
{
    [SerializeField] public int skillPoints;
    [SerializeField] public TextMeshProUGUI skillPointText;
    [SerializeField] UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }


    private void Start()
    {

        UpdateAllConnections();
        UpdateSkillPointUI();
    }

    private void UpdateSkillPointUI()
    {
        skillPointText.text = skillPoints.ToString();
    }

    public void UnlockDefaulSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();
        foreach (var node in allTreeNodes)
        {
            node.UnlockDefaulSkill();
        }
    }

[ContextMenu("Reset Skill Tree")]
    public void RefundAllSkillPoints()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in skillNodes)
            node.Refund();
        
    }

    public bool EnoughSkillPoints(int cost)
    {
        return skillPoints >= cost;
    }

    public void RemoveSkillPoint(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointUI();
    }
    public void AddSkillPoint(int amount)
    {
        skillPoints += amount;
        UpdateSkillPointUI();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints;

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.skillName;

            if (data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUnlock.TryGetValue(skill.GetSkillType(), out SkillUnlock_Type upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);

                if (upgradeNode != null)
                    skill.SetSkillUnlock(upgradeNode.skillData);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUnlock.Clear();

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.skillName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }


        foreach (var skill in skillManager.allSkills)
        {
            data.skillUnlock[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}
