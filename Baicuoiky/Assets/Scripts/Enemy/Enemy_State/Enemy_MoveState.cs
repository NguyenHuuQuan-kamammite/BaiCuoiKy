using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    private Entity_SFX sfx;

    private float stepTimer;
    private float stepInterval = 0.5f;
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        sfx = enemy.GetComponent<Entity_SFX>();
        
    }
    public override void Enter()
    {
        base.Enter();
        stepTimer = 0f; // Reset timer when entering move state
        if (enemy.groundDetected == false || enemy.wallDetected)
        {
            enemy.Flip();
        }
    }
    public override void Update()
    {
        base.Update();
        

        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            sfx?.PlayWalkStone();
            stepTimer = stepInterval;
        }
        enemy.SetVelocity(enemy.GetMoveSpeed() * enemy.facingDir, rb.linearVelocity.y);
        if (enemy.groundDetected == false || enemy.wallDetected)

        {
            stateMachine.ChangeState(enemy.idleState);

        }
    }
}