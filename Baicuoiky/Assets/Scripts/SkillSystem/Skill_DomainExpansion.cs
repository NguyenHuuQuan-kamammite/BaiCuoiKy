using UnityEngine;

public class Skill_DomainExpansion : Skill_Base

{


    [SerializeField] private GameObject domainPrefab;

    [Header("Domain slowdow upgrade details")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5f;


    [Header("Spell Casting Upgrade")]
    [SerializeField] private float spellCastingDomainSlowDown = 1;
    [SerializeField] private float spellCasingDomainDuration = 8;


    [Header("Domain details")]
    public float maxDomainSize = 10f;
    public float expandSpeed = 3f;

    public float GetDomainDuration()
{
    if (unlockType == SkillUnlock_Type.Domain_SlowingDown)
        return slowDownDomainDuration;
    else
        return spellCasingDomainDuration;
}

public float GetSlowPercentage()
{
    if (unlockType == SkillUnlock_Type.Domain_SlowingDown)
        return slowDownPercent;
    else
        return spellCastingDomainSlowDown;
}

    public bool InstantDomain()
    {
        return unlockType != SkillUnlock_Type.Domain_EchoSpam
            && unlockType != SkillUnlock_Type.Domain_ShardSpam;
    }
    public void CreateDomain()
    {
        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetUpDomain(this);
    }

}
