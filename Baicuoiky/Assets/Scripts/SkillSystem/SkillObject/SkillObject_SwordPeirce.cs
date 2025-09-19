using UnityEngine;

public class SkillObject_SwordPeirce : SkillObject_Sword
{
    private int amountToPeirce;
    public override void SetUpSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetUpSword(swordManager, direction);
        amountToPeirce = swordManager.peirceAmount;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
        if (amountToPeirce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform, .3f);
            StopSword(collision);
            return;
        }
        amountToPeirce--;
        DamageEnemiesInRadius(transform, .3f);

    }
}
