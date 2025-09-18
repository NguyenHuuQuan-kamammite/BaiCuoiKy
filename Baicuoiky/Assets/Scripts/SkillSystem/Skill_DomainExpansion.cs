using UnityEngine;

public class Skill_DomainExpansion : Skill_Base

{
    public bool InstantDomain()
    {
        return unlockType != SkillUnlock_Type.Domain_EchoSpam
            && unlockType != SkillUnlock_Type.Domain_ShardSpam;
    }
    public void CreateDomain()
    {
        Debug.Log("Create Skill Object");
    }

}
