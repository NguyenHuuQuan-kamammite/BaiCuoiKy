using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }

    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }
    public Skill_DomainExpansion domainExpansion{ get; private set; }
    private Skill_Base[] allSkills;
    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();
        domainExpansion = GetComponentInChildren<Skill_DomainExpansion>();
        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCoolDownBy(amount);
    }
    public Skill_Base GetSkillByType(Skill_Type type)
    {
        switch (type)
        {
            case Skill_Type.Dash:
                return dash;
            case Skill_Type.TimeShard:
                return shard;
            case Skill_Type.SwordThrow:
                return swordThrow;
            case Skill_Type.TimeEcho:
                return timeEcho;
            case Skill_Type.DomainExpansion:
                return domainExpansion;
            default:
                Debug.LogError("Skill type not recognized");
                return null;
        }
    }
}
