using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject onDeathVFX;
    private Skill_TimeEcho echoManager;
    public int maxAttack{ get; private set; }
    public void SetUpEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        FlipToTarget();
        maxAttack = echoManager.GetMaxAttack();
        anim.SetBool("canAttack", maxAttack > 0);

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
    }
    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }
    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }
    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);
        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }
    
   public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);
        if (hit.collider != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}
