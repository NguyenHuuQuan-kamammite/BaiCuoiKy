using UnityEngine;
using System.Collections;


public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2f;

    [Header("Shard Moving")]
    [SerializeField] private float shardSpeed = 7f;
    [Header("Multicast shard upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;
    [Header("Teleport shard upgrade")]
    [SerializeField] private float shardExistDuration = 10f;
    [Header("Health Rewind Shard upgrade")]
    [SerializeField] private float saveHealthPercent;
    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
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
        if (unlockType == SkillUnlock_Type.Shard_Teleport)
        {
            HandleShardTeleport();
        }
        if (unlockType == SkillUnlock_Type.Shard_TeleportHPRewind)
        {
            HandleShardHealthRewind();
        }

    }
    private void HandleShardHealthRewind()
    {
         if (currentShard == null)
        {
            CreateShard();
            saveHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(saveHealthPercent);
            SetSkillOnCooldown();
        }
    }
    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();

        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }
    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;
        currentShard.transform.position = playerPosition;
        player.TeleportPlayer(shardPosition);
        currentShard.Explode();
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
        float detonationTime = GetDetonateTime();
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);
        if (Unlocked(SkillUnlock_Type.Shard_Teleport) || Unlocked(SkillUnlock_Type.Shard_TeleportHPRewind))
        {
            currentShard.OnExplode += ForceCooldown;
        }
    }

    public void CreateRawShard()
    {
        bool canMove = Unlocked(SkillUnlock_Type.Shard_MoveToEnemy) || Unlocked(SkillUnlock_Type.Shard_MultiCast);
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetUpShard(this, detonationTime, canMove, shardSpeed);
    }
    public float GetDetonateTime()
    {
        if (Unlocked(SkillUnlock_Type.Shard_Teleport) || Unlocked(SkillUnlock_Type.Shard_TeleportHPRewind))
        {
            return shardExistDuration;
        }
        return detonationTime;
    }
    private void ForceCooldown()
    {
        if (OnCoolDown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
    