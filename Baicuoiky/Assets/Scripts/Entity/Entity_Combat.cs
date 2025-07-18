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

            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);
            if(element != ElementType.None)
            {
                ApplyStatusEffect(target.transform, element);
            }
            if (targetGotHit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element , float scaleFactor = 1f)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (statusHandler == null) return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
          statusHandler.ApplyChillEffect(defaultDuration, chillSlowMultiplier* scaleFactor); // Apply scale factor to chill effect
        if (element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {   

            scaleFactor = fireScale ; // Apply scale factor to fire damage
            float fireDamage = stats.offense.fireDamage.GetValue()* scaleFactor; // Apply scale factor to fire damage
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }
        if (element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
        {
            scaleFactor = lightningScale; // Apply scale factor to lightning damage
            float lightningDamage = stats.offense.lightningDamage.GetValue() * scaleFactor; // Apply scale factor to lightning damage
            statusHandler.ApplyElectricEffect(defaultDuration, lightningDamage, electricCharge); // Apply scale factor to charge
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
