using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    [SerializeField] UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }


    private void Start()
    {

        UpdateAllConnections();
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
    }
    public void AddSkillPoint(int amount)
    {
        skillPoints += amount;
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
