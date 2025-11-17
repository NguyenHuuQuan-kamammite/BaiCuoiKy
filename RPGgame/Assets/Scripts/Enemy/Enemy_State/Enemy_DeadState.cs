using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public GameObject healthBar;

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"{enemy.name} has died.");
        if (enemy.healthBarUI != null)
        {
            enemy.healthBarUI.SetActive(false); // Hide it
        }
        stateMachine.SwichOffState();
        enemy.DestroyGameObjectWithDelay(); 
    }
}
