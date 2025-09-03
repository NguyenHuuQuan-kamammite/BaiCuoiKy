using UnityEngine;
using System;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;
    private void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    public void MoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }
    public void SetupShard(float detinationTime)    
    {
        Invoke(nameof(Explode), detinationTime);
    }
    public void Explode()
    {
        DamageEnemiesInRadius(transform, targetCheckRadius);
        OnExplode?.Invoke();
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;


        Explode();
    }
}
