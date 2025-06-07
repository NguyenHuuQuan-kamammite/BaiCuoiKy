using System;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        if (player == null)
        {
            player = enemy.PlayerDetected().transform;
        }
    }
    public override void Update()
    {
        base.Update();
        if (WithInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
           enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);

    }
    private bool WithInAttackRange()
    {
        return DisTanceToPlayer() < enemy.attackDistance;
    }
    private float DisTanceToPlayer()
    {
        if (player == null)
        {
            return float.MaxValue; // or some large value
        }
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    private int DirectionToPlayer()
    {
        if (player == null)
        {
            return 0; // or some default value
        }
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}

