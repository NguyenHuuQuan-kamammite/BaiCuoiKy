using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D playerHit = enemy.PlayerDetected();

        if (playerHit)
        {
            // Use TryEnterBattleState to properly set player reference
            enemy.TryEnterBattleState(playerHit.transform);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
