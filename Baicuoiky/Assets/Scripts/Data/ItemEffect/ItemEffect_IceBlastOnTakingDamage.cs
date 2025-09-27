using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Ice Blast ", fileName = "Item Effect data- IceBlast on Taking Damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;


    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown ;
    private float lastTimeUsed = -999;
    [Header("Ice Blast Effect")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;
        if (noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }
    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);
        foreach (var target in enemies)
        {
            IDamgable damgable = target.GetComponent<IDamgable>();
            if (damgable == null) continue;
            bool targetGotHit = damgable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);
            if (targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
               

        }
    }
    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.health.OnTakingDamage += ExecuteEffect;

    }
    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
    
}
