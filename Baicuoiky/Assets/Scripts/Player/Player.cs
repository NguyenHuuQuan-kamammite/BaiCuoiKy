using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{

    public static event Action OnPlayerDeath;
    private UI ui;
    public Player_SkillManager skillManager { get; private set; }
    public Player_Vfx vfx { get; private set; }
    [Header("Attack details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    [Header("Ultimate abilty details")]
    public float riseSpeed = 25f;
    public float riseMaxDistance = 3f;







    [Header("Movement details")]
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;

    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f; // Should be from 0 to 1;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = .7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20;

    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }
    public Vector2 mousePosition { get; private set; }

    #region States
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_Dead deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_ThrowSwordState throwSwordState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        ui = FindAnyObjectByType<UI>();
        skillManager = GetComponent<Player_SkillManager>();
        vfx = GetComponent<Player_Vfx>();
        health = GetComponent<Entity_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();


        input = new PlayerInputSet();


        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_Dead(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        throwSwordState = new Player_ThrowSwordState(this, stateMachine, "swordThrow");
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, "jumpFall");

    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }
    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalSpeed = moveSpeed;
        float originalAnimSpeed = anim.speed;
        float originalJumpForce = jumpForce;
        Vector2 originalWallJumpForce = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = attackVelocity;


        float speedMultiplier = 1f - slowMultiplier;
        moveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        wallJumpForce *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        anim.speed = originalAnimSpeed;
        jumpForce = originalJumpForce;
        wallJumpForce = originalWallJumpForce;
        jumpAttackVelocity = originalJumpAttack;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

        public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }
    private void OnEnable()
    {
        input.Enable();


        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTree();
        input.Player.Spell.performed += ctx => skillManager.shard.TryToUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryToUseSkill();

    }

    private void OnDisable()
    {
        input.Disable();
    }
    
}
