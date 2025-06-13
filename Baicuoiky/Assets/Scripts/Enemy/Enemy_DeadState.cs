using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log($"{enemy.name} has died.");
        stateMachine.SwichOffState();
    }
}
