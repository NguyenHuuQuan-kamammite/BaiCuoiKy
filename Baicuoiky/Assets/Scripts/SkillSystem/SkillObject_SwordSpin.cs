using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int maxDistance;
    private float attackPerSecond;
    private float attackTimer;

public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetUpSword(swordManager, direction);
        anim?.SetTrigger("spin");
        maxDistance = swordManager.maxDistance;
        attackPerSecond = swordManager.attackPerSecond;
        Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
    }
protected override void Update()
    {
        
        HandleAttack();
        HandleStopping();
        HandleComeBack();
    }   
    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > maxDistance && rb.simulated == true)
        {
            rb.simulated = false;
        }
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0f)
        {
            DamageEnemiesInRadius(transform, .3f);
            attackTimer = 1f / attackPerSecond;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
       rb.simulated = false;
    }
}
