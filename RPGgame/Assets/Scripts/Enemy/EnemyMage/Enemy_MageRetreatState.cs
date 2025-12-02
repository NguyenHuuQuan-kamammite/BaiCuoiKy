using UnityEngine;

public class Enemy_MageRetreatState : EnemyState
{
    private Enemy_Mage enemyMage;
    private Vector3 startPosition;
    private Transform player;

    public Enemy_MageRetreatState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage;
    }

    public override void Enter()
    {
        base.Enter();

        player = enemy.GetPlayerReference();

        if (player == null)
        {
            
            stateMachine.ChangeState(enemyMage.mageBattleState);
            return;
        }

        startPosition = enemy.transform.position;

        // Flip FIRST to face the player
        enemy.HandleFlip(DirectionToPlayer());

        // Set velocity directly WITHOUT using SetVelocity (to avoid HandleFlip being called again)
        rb.linearVelocity = new Vector2(enemyMage.retreatSpeed * -enemy.facingDir, 0);
        enemy.MakeUntargetable(false);
        enemy.vfx.DoImageEchoEffect(1f);
    }

    public override void Update()
    {
        base.Update();

        // Keep moving backwards (away from current facing direction)
        rb.linearVelocity = new Vector2(enemyMage.retreatSpeed * -enemy.facingDir, rb.linearVelocity.y);

        float currentDistance = Vector2.Distance(enemy.transform.position, startPosition);
        bool reachedMaxDistance = currentDistance >= enemyMage.retreatMaxDistance;
        bool cantMoveBack = enemyMage.CantMoveBackwards();

        if (reachedMaxDistance || cantMoveBack)
        {
            stateMachine.ChangeState(enemyMage.mageSpellCastState);
        }
    }

    public override void Exit()
    {

        base.Exit();
        rb.linearVelocity = Vector2.zero;
        enemy.vfx.StopImageEchoEffect();
        enemy.MakeUntargetable(true);
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
        {
            return enemy.facingDir;
        }
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
