using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Skill_DomainExpansion : Skill_Base

{


    [SerializeField] private GameObject domainPrefab;

    [Header("Domain slowdow upgrade details")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5f;


    [Header("Spell Casting Upgrade")]
    [SerializeField] private int spellToCast = 10;
    [SerializeField] private float spellCastingDomainSlowDown = 1;
    [SerializeField] private float spellCasingDomainDuration = 8;
    private float spellCastTimer;
    private float spellsPerSecond;

    [Header("Domain details")]
    public float maxDomainSize = 10f;
    public float expandSpeed = 3f;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTargget;

     public void CreateDomain()
    {
        spellsPerSecond = spellToCast / GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetUpDomain(this);
    }
    public void DoSpellCastiing()
    {
        spellCastTimer -= Time.deltaTime;
        if (currentTargget == null)
        {
            currentTargget = FindTargetInDomain();
        }
        if (currentTargget != null && spellCastTimer < 0)
        {
            CastSpell(currentTargget);
            spellCastTimer = 1 / spellsPerSecond;
            currentTargget = null;
        }
    }
    private void CastSpell(Transform target)
    {
        if (unlockType == SkillUnlock_Type.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1, 0) : new Vector2(-1, 0);

            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        if (unlockType == SkillUnlock_Type.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target,true);
        }
    }

    private Transform FindTargetInDomain()
    {
        if (trappedTargets.Count == 0)
            return null;
        int randomIndex = Random.Range(0, trappedTargets.Count);
        Transform target = trappedTargets[randomIndex].transform;
        if (target == null)
        {
            trappedTargets.RemoveAt(randomIndex);
            return null;
        }
        return target;

    }
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
   
    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);


    }
    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();

        trappedTargets = new List<Enemy>();
    }
}
