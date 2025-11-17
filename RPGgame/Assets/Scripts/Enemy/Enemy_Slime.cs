using System.Collections;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy_Slime : Enemy, ICounterable
{

    public Enemy_SlimeDeadState slimeDeadState { get; set; }
    public bool CanBeCountered { get => canBeStunned; }
    [Header("Slime specifics")]

    [SerializeField] private GameObject slimeToCreatePrefab;
    [SerializeField] private int amountOfSlimesToCreate = 2;

    [SerializeField] private bool hasRecoveryAnimation = true;
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        slimeDeadState = new Enemy_SlimeDeadState(this, stateMachine, "dead");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        anim.SetBool("hasStunRecovery", hasRecoveryAnimation);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }




    public override void EntityDeath()
    {
        stateMachine.ChangeState(slimeDeadState);
    }
    [ContextMenu("Stun Enemy")]
    public void HandleCounter()
    {
        if (CanBeCountered == false)
        {
            return; 
        }
        stateMachine.ChangeState(stunnedState);
    }




    public void CreateSlimeOnDeath()
    {
        if (slimeToCreatePrefab == null)
            return;

        for (int i = 0; i < amountOfSlimesToCreate; i++)
        {
            GameObject newSlime = Instantiate(slimeToCreatePrefab, transform.position, Quaternion.identity);
            Enemy_Slime slimeScript = newSlime.GetComponent<Enemy_Slime>();

            slimeScript.stats.AdjustStatSetup(stats.resources, stats.offense, stats.defense, .6f, 1.2f);
            slimeScript.ApplyRespawnVelocity();

            // Small delay to ensure Awake/Start methods are called
            StartCoroutine(DelayedStartBattleState(slimeScript, player));
        }
    }
    private IEnumerator DelayedStartBattleState(Enemy_Slime slime, Transform playerTarget)
    {
        // Wait one frame to ensure all initialization is complete
        yield return null;
        slime.StartBattleStateCheck(playerTarget);
    }

    public void ApplyRespawnVelocity()
    {
        Vector2 velocity = new Vector2(stunnedVelocity.x * Random.Range(-1f, 1f), stunnedVelocity.y * Random.Range(1f, 2f));
        SetVelocity(velocity.x, velocity.y);
    }
    public void StartBattleStateCheck(Transform player)
    {
        if (stateMachine.currentState == null)
        {
            stateMachine.Initialize(idleState);
        }

        TryEnterBattleState(player);
        InvokeRepeating(nameof(ReEnterBattleState), 0, .3f);
    }
    private void ReEnterBattleState()
    {
        if (stateMachine?.currentState == null)
            return;
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            CancelInvoke(nameof(ReEnterBattleState));
            return;
        }

        stateMachine.ChangeState(battleState);
    }
}
