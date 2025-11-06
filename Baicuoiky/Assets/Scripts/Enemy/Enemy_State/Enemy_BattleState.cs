using System;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform lastTarget;
    protected float lastTimeWasInBattle;
    protected float lastTimeAttacked = float.NegativeInfinity;


    private Entity_SFX sfx;
    private float stepTimer;
    private float stepInterval = 0.4f; 
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        sfx = enemy.GetComponent<Entity_SFX>();
    }
    public override void Enter()
    {
        base.Enter();
        stepTimer = 0f;
        UpdateBattleTimer();
        if (player == null)
        {
            player = enemy.GetPlayerReference(); // Get the player reference from the enemy
        }
        
        if (ShouldRetreat())
        {
            ShortRetreat();
        }

    }

    public void ShortRetreat()
    {
        float x = (enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer();
        float y = enemy.retreatVelocity.y;
        rb.linearVelocity = new Vector2(x, y);
        enemy.HandleFlip(DirectionToPlayer());
    }

    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }
        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);

        }
        if (WithInAttackRange() && enemy.PlayerDetected() == true && CanAttack())
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            float xVeloicty = enemy.canChasePlayer ? enemy.GetBattleMoveSpeed() : 0.0001f;
            enemy.SetVelocity(xVeloicty * DirectionToPlayer(), rb.linearVelocity.y);

            if (enemy.canChasePlayer && xVeloicty > 0.1f)
            {
                // Calculate interval based on battle speed
                float currentInterval = Mathf.Max(0.2f, 0.4f / Mathf.Max(enemy.GetBattleMoveSpeed(), 1f));

                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    sfx?.PlayWalkStone();
                    stepTimer = currentInterval;
                }

            }
        }
    }
    protected bool CanAttack() => Time.time > lastTimeAttacked + enemy.attackCooldown;
    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;
       Transform newTarget = enemy.PlayerDetected().transform;
        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }
    protected float UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    protected bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;
    protected bool ShouldRetreat() => DisTanceToPlayer() < enemy.minRetreatDistance;

    protected bool WithInAttackRange()
    {
        return DisTanceToPlayer() < enemy.attackDistance;
    }
    protected float DisTanceToPlayer()
    {
        if (player == null)
        {
            return float.MaxValue; // or some large value
        }
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    protected int DirectionToPlayer()
    {
        if (player == null)
        {
            return 0; // or some default value
        }
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}

