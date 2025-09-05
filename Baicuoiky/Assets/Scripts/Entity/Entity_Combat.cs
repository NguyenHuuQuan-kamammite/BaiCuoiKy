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
            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            bool targetGotHit = damagable.TakeDamage(physDamage, elementalDamage, element, transform);
            if(element != ElementType.None)
            {
              statusHandler?.ApplyStatusEffect(element, attackData.effectData);
            }
            if (targetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
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
