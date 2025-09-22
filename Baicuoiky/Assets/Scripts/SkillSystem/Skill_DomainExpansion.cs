using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Skill_DomainExpansion : Skill_Base

{


    [SerializeField] private GameObject domainPrefab;

    [Header("Domain slowdow upgrade details")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5f;


    [Header("Shard Casting Upgrade")]
    [SerializeField] private int shardToCast = 10;
    [SerializeField] private float shardCastingDomainSlowDown = 1;
    [SerializeField] private float shardCasingDomainDuration = 8;

    [Header("Time Echo Casting Upgrade")]
    [SerializeField] private int echoToCast = 8;
    [SerializeField] private float echoCastingDomainSlowDown = 1;
    [SerializeField] private float echoCasingDomainDuration = 6;
    [SerializeField] private float healthToRestoreWithEcho = .05f;


    private float spellCastTimer;
    private float spellsPerSecond;

    [Header("Domain details")]
    public float maxDomainSize = 10f;
    public float expandSpeed = 3f;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTargget;

     public void CreateDomain()
    {
        spellsPerSecond = GetSpellToCast() / GetDomainDuration();
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
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if (trappedTargets.Count == 0)
            return null;
        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;

    }
    public float GetDomainDuration()
    {
        if (unlockType == SkillUnlock_Type.Domain_SlowingDown)
            return slowDownDomainDuration;
        if (unlockType == SkillUnlock_Type.Domain_SlowingDown)
            return slowDownPercent;
        else if (unlockType == SkillUnlock_Type.Domain_ShardSpam)
            return shardCasingDomainDuration;
        else if (unlockType == SkillUnlock_Type.Domain_EchoSpam)
            return echoCasingDomainDuration;
        return 0;
    }

    public float GetSlowPercentage()
    {
        if (unlockType == SkillUnlock_Type.Domain_SlowingDown)
            return slowDownPercent;
        else if (unlockType == SkillUnlock_Type.Domain_ShardSpam)
            return shardCastingDomainSlowDown;
        else if (unlockType == SkillUnlock_Type.Domain_EchoSpam)
            return echoCastingDomainSlowDown;
        return 0;
    }
    private int GetSpellToCast()
    {
        if (unlockType == SkillUnlock_Type.Domain_ShardSpam)
            return shardToCast;
        else if (unlockType == SkillUnlock_Type.Domain_EchoSpam)
            return echoToCast;
        return 0;
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
