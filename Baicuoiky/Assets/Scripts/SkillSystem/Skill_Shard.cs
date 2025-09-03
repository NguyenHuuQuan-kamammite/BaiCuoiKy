using UnityEngine;
using System.Collections;
public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2f;

    [Header("Shard Moving")]
    [SerializeField] private float shardSpeed = 7f;
    [Header("Multicast shard upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
    }
    public override void TryToUseSkill()
    {
        if (CanUseSkill() == false)
        {
            return;
        }

        if (unlockType == SkillUnlock_Type.Shard)
        {
            HandleShardRegular();
        }
        if (unlockType == SkillUnlock_Type.Shard_MoveToEnemy)
        {
            HandleShardMoving();
        }
        if (unlockType == SkillUnlock_Type.Shard_MultiCast)
        {
            HandleShardMultiCast();
        }


    }

    private void HandleShardMultiCast()

    {
        if (currentCharges <= 0)
        {
            return;
        }

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;
        if (isRecharging == false)
            StartCoroutine(RechargeShardCo());
    }
    private IEnumerator RechargeShardCo()
    {
        isRecharging = true;
        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(coolDown);
            currentCharges++;
        }
        isRecharging = false;
    }
    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(detonationTime);
    }
}
