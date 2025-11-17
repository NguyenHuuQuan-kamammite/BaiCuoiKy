using UnityEngine;

public class Enemy_AnimationTriggers : Enity_AnimationTriggers
{

    private Enemy enemy;
    private Enemy_VFX enemyVFX;
    override protected void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<Enemy_VFX>();
    }
    private void SpecialAttackTrigger()
    {
        enemy.SpecialAttack();
    }

    private void EnableCounterWindow()
    {
        enemyVFX.EnableAttackAlert(true);
        enemy.EnableCounterWindows(true);
    }
    private void DisableCounterWindow()
    {
        enemyVFX.EnableAttackAlert(false);
        enemy.EnableCounterWindows(false);
    }
}

