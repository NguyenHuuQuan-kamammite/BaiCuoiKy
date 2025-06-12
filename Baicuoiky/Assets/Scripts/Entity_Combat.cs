using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
 
    [Header(("Target detection"))]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1f;
    [SerializeField] private LayerMask WhatIsTarget;


    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
         Debug.Log("Attacking " + target.name);
        }
    }
    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, WhatIsTarget);
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
   
}
