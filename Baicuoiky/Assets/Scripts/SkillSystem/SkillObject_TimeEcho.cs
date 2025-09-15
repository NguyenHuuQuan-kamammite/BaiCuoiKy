using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject onDeathVFX;
    private Skill_TimeEcho echoManager;
    public void SetUpEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
      
        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
    }
    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
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
