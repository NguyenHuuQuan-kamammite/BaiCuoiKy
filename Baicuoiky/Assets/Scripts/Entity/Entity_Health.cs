using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamgable
{
    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;

    private Entity_Stats stats;




    [SerializeField] protected float currentHp;
    
    [SerializeField] protected bool isDead;
    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1f, 0.5f);
    [Header("Heavy Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyKnockbackThreshold = .3f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyKnockback = new Vector2(7f, 7f);
    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return;
        float duration = CalcutateKnockbackDuration(damage);
        Vector2 knockback = CalculateKnockbackDirection(damage, damageDealer);

        entityVFX?.PlayOnDamageVFX();
        entity?.ReceiveKnockback(knockback, duration);
        ReduceHp(damage);
    }
    protected void ReduceHp(float damage)
    {
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        isDead = true;

        entity.EntityDeath();
    }
    private void UpdateHealthBar()
    { 
        if (healthBar == null) return;
        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    private Vector2 CalculateKnockbackDirection(float damage, Transform damageDealer)
    {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyKnockback : onDamageKnockback;
        knockback.x *= direction;
        return knockback;
    }
    private float CalcutateKnockbackDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }
    private bool IsHeavyDamage(float damage)
    {
        float healthPercentTaken = damage / stats.GetMaxHealth();
        return healthPercentTaken > heavyKnockbackThreshold;
    }
}