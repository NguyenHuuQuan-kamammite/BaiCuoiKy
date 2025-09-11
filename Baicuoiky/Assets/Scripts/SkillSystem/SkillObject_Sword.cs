using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;
    protected Rigidbody2D rb;


    protected Transform playerTransform ;
    protected float comebackSpeed = 20f;
    protected bool shouldComeBack;
    protected float maxAllowedDistance = 25f;

   protected virtual void Update()
    {
        transform.right = rb.linearVelocity;
        HandleComeBack();
    }
    public virtual void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction;

        this.swordManager = swordManager;

        playerTransform = swordManager.transform.root;
        playerStats = swordManager.player.stats;
        damageScaleData = swordManager.damageScaleData;
    }
public void GetSwordBackToPlayer()
    {
        shouldComeBack = true;
        
    }
    protected void HandleComeBack()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance > maxAllowedDistance)
        {
           GetSwordBackToPlayer();
        }
        if (shouldComeBack == false)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed * Time.deltaTime);
        if (distance < 0.5f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);

        DamageEnemiesInRadius(transform, 1f);
    }
    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
