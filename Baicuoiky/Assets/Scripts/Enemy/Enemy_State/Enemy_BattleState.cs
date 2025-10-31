using System;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform lastTarget;
    protected float lastTimeWasInBattle;
    protected float lastTimeAttacked = float.NegativeInfinity;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        UpdateBattleTimer();
        if (player == null)
        {
            player = enemy.GetPlayerReference(); // Get the player reference from the enemy
        }
        
        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2((enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }

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
        if (WithInAttackRange()  && enemy.PlayerDetected() == true && CanAttack())     
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            float xVeloicty = enemy.canChasePlayer ? enemy.GetBattleMoveSpeed() : 0.0001f;
            enemy.SetVelocity(xVeloicty * DirectionToPlayer(), rb.linearVelocity.y);
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

