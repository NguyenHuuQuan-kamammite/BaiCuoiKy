using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{

    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;
    [Header("Battle Details")]
    public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;
    public float battleTimeDuration = 5f;
    public float minRetreatDistance = 1f;
    public Vector2 retreatVelocity;

    [Header("Movement Details")]
    public float moveSpeed = 1.4f;
    public float idleTime = 1f;

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1f;


    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;


    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10f;
    public Transform player { get; private set; }
    public float activeSlowMultiplier { get; private set; } = 1;
    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier;
    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier;

    [Header("Stunned Details")]
    public float stunnedDuration = 1f;
    public Vector2 stunnedVelocity  = new Vector2(7, 7);
    [SerializeField]protected bool canBeStunned;
    [HideInInspector] public GameObject healthBarUI;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
      

        activeSlowMultiplier = 1f - slowMultiplier;

      
        anim.speed *= activeSlowMultiplier;
        yield return new WaitForSeconds(duration);
   
    }


    public override void StopSlowDown()
    {
        activeSlowMultiplier = 1;
        anim.speed = 1;
        base.StopSlowDown();
    }
       protected override void Awake()
    {
        base.Awake();
        // Find the child named "HealthBar_UI"
        healthBarUI = transform.Find("HealthBar_UI")?.gameObject;
    }


    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return; // Already in battle or attack state
        }
        this.player = player;
        stateMachine.ChangeState(battleState);

    }
    public void EnableCounterWindows(bool enable)
    {
        canBeStunned = enable;
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }
    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }
    public Transform GetPlayerReference()
    {
        if (player == null)
        {
            player = PlayerDetected().transform;
        }
        return player;
    }
    public RaycastHit2D PlayerDetected()

    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Debug.DrawRay(playerCheck.position, Vector2.right * facingDir * playerCheckDistance, Color.red);
            return default;
        }

        return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (playerCheckDistance * facingDir), playerCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (attackDistance * facingDir), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (minRetreatDistance * facingDir), playerCheck.position.y));
    }
    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
