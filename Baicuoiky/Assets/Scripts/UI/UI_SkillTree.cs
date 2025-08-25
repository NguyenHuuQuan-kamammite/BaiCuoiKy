using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    [SerializeField] UI_TreeConnectHandler[] parentNodes;
    public Player_SkillManager skillManager { get; private set; }

private void Awake()
    {
       skillManager = FindAnyObjectByType<Player_SkillManager>();
    }
    private void Start()
    {

        UpdateAllConnections();
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
