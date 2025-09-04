using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    private Entity_VFX vfx;

    public DamageScaleData basicAttackScale;
    private Entity_Stats stats;
 
    [Header(("Target detection"))]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1f;
    [SerializeField] private LayerMask WhatIsTarget;

    [Header("Status Effects details")]
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = 0.2f;
    [SerializeField] private float electricCharge = 0.4f;
    [Space]
    [SerializeField] private float fireScale = 0.8f;
    
    [SerializeField] private float lightningScale = 2.5f;
    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamgable damagable = target.GetComponent<IDamgable>();
            if (damagable == null) continue;
            float elementalDamage = stats.GetElementalDamage(out ElementType element, 0.6f);
            float damage = stats.GetPhysicalDamage(out bool isCrit);


            ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);
            
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);
            if(element != ElementType.None)
            {
              target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effectData);
            }
            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }



    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, WhatIsTarget);
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
   
}
