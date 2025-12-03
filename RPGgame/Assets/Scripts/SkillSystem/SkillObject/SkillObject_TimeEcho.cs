using Unity.VisualScripting;
using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject onDeathVFX;
    private bool shouldMoveToPlayer;

    private Transform playerTranform;
    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;
    private Entity_Health playerhealth;
    private SkillObject_Health echoHealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;
    private bool hasHealed;
    [Header("Sound Effects")]
    [SerializeField] private string wispAbsorbSFX = "wisp_absorb"; 
    [SerializeField] private float soundDistance = 15f;
    private AudioSource audioSource;
    public int maxAttack { get; private set; }
    public void SetUpEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttack = echoManager.GetMaxAttack();
        playerTranform = echoManager.transform.root;
        playerhealth = echoManager.player.health;
        statusHandler = echoManager.player.statusHandler;
        skillManager = echoManager.skillManager;

        FlipToTarget();
        anim.SetBool("canAttack", maxAttack > 0);
        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Update()
    {
        if (shouldMoveToPlayer)
        {
            HandleWispMovement();
        }
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }


        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }

    private void HandlePlayerTouch()
    {
        Debug.Log("Wisp touched player!"); // Check if this prints
        if (hasHealed) // Prevent multiple calls
            return;

        hasHealed = true;

        if (audioSource == null)
            Debug.LogError("AudioSource is null!");
        else
            Debug.Log("AudioSource exists, playing sound: " + wispAbsorbSFX);
        Audio_Manager.instance.PlaySFX(wispAbsorbSFX, audioSource, soundDistance);
        float healthAmount = echoHealth.lastDamegeTaken * echoManager.GetPercentOfDamageHealed();
        playerhealth.IncreaseHealth(healthAmount);

        float amountInSecond = echoManager.GetCoolDownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSecond);

        if (echoManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffect();

    }
    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTranform.position, wispMoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, playerTranform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject, 0.5f);
        }
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
        if (echoManager.ShouldBeWisp())
        {
            TurnIntoWisp();
        }
        else
            Destroy(gameObject);
    }
    private void TurnIntoWisp()
    {
            shouldMoveToPlayer = true;
            anim.gameObject.SetActive(false);
            wispTrail.gameObject.SetActive(true);
            rb.simulated = false;
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
