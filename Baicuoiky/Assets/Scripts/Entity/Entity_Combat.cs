using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    private Entity_VFX vfx;

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
            float elementalDamage = stats.GetElementalDamage(out ElementType element);
            float damage = stats.GetPhysicalDamage(out bool isCrit);

            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);
            if (targetGotHit) 
             vfx.CreateOnHitVFX(target.transform, isCrit);
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
