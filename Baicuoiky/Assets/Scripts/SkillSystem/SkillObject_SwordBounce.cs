using UnityEngine;
using System.Collections.Generic;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] private float bounceSpeed = 15f;
    private int bounceCount;
    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();
    public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        anim.SetTrigger("spin");
        base.SetUpSword(swordManager, direction);
        bounceCount = swordManager.bounceCount;
        bounceSpeed = swordManager.bounceSpeed;
       
    }
protected override void Update()
    {
        HandleBounce();
        HandleComeBack();
    }
    private void HandleBounce()
    {
        if (nextTarget == null)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, nextTarget.position) < 0.75f)
        {
            DamageEnemiesInRadius(transform, 1f);
            BounceToNextTarget();
            if (bounceCount == 0 || nextTarget == null)
            {
                nextTarget = null;
                GetSwordBackToPlayer();
            }
        }
    }
    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
       
        bounceCount--;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = EnemiesAround(transform, 10f);
            rb.simulated = false;
        }
        DamageEnemiesInRadius(transform, 1f);
        if (enemyTargets.Length <= 1 || bounceCount == 0)
        {
            GetSwordBackToPlayer();

        }
        else
        {
           nextTarget = GetNextTarget();
        }
    }
    private Transform GetNextTarget()
    {
        List<Transform> validTarget = GetValidTarget();
        if (validTarget.Count == 0)
            return null;
        int randomIndex = Random.Range(0, validTarget.Count);
        Transform nextTarget = validTarget[randomIndex];
        selectedBefore.Add(nextTarget);
        return nextTarget;
    }
    private List<Transform> GetValidTarget() 
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTarget();
        foreach (var enemy in aliveTargets)
        {
            if (enemy != null && !selectedBefore.Contains(enemy.transform))
            {
                validTargets.Add(enemy.transform);
            }
        }
        if (validTargets.Count > 0)
        {
            return validTargets;
        }
        else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }
    }
    private List<Transform> GetAliveTarget()
    {
        List<Transform> aliveTargets = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
            {
                aliveTargets.Add(enemy.transform);
            }
        }
        return aliveTargets;
    }

}
