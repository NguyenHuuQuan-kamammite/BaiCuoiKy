using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10f;
    private Entity_VFX vfx;
 
    [Header(("Target detection"))]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1f;
    [SerializeField] private LayerMask WhatIsTarget;
    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        
    }

    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamgable damagable = target.GetComponent<IDamgable>();
            if (damagable == null) continue;
            
            damagable.TakeDamage(damage, transform);
            vfx.CreateOnHitVFX(target.transform);
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
