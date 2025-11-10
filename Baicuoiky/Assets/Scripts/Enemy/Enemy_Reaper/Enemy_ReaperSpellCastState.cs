using UnityEngine;

public class Enemy_ReaperSpellCastState : EnemyState
{
    private Enemy_Reaper enemyReaper;
    private float stateTimer;
    private float maxStateDuration = 10f;
    public Enemy_ReaperSpellCastState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyReaper = enemy as Enemy_Reaper;
    }
    public override void Enter()
    {
        base.Enter();

        enemyReaper.SetVelocity(0, 0);
        enemyReaper.SetSpellCastPreformed(false);
        enemyReaper.SetSpellCastOnCooldown();
        enemyReaper.MakeUntargetable(false);
        enemyReaper.EnableCounterWindows(true);
    }
    public override void Update()
    {
        base.Update();


        stateTimer += Time.deltaTime;

      
        if (enemyReaper.spellCastPreformed || stateTimer >= maxStateDuration)
        {
            anim.SetBool("spellCast_Performed", true);
        }

        
        if (triggerCalled)
        {
            if (enemyReaper.ShouldTeleport())
                stateMachine.ChangeState(enemyReaper.reaperTeleportState);
            else
                stateMachine.ChangeState(enemyReaper.reaperBattleState);
        }
    }

    
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("spellCast_Performed", false);
        enemyReaper.StopAllCoroutines();
        enemyReaper.MakeUntargetable(true);
    }
}
