using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamgable
{
    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;

    private Entity_Stats stats;

    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegenerateHealth = true;

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
        InvokeRepeating(nameof(RegenerateHealth),0, regenInterval);
    }

    public virtual bool TakeDamage(float damage,float elementalDamage,ElementType element, Transform damageDealer)
    {
        if (isDead) return false;
        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;

        float mitigation = stats.GetArmorMitigation(armorReduction);

        float physicalDamageTaken = damage * (1 - mitigation);
        float resistance = stats.GetElementalResistance(element);

        float elementalDamageTaken = elementalDamage * (1 - resistance);

        Debug.Log($"{gameObject.name} took {physicalDamageTaken} physical damage and {elementalDamageTaken} elemental damage from {damageDealer.name} + total damage: {physicalDamageTaken + elementalDamageTaken}");

        TakeKnockback(damageDealer, physicalDamageTaken);
        
        ReduceHp (physicalDamageTaken + elementalDamageTaken);
        
        return true;
    }

    public void IncreaseHealth(float healthAmount)
    {
        if (isDead) return;
        float newHealth = currentHp + healthAmount;
        float maxHealth = stats.GetMaxHealth();
        currentHp = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }
    private void RegenerateHealth()
    {
        if (isDead || !canRegenerateHealth) return;

        float regenAmount = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    private bool AttackEvaded()
    {
        return Random.Range(0f, 100f) < stats.GetEvasion();
    }
    public void ReduceHp(float damage)
    {
        entityVFX?.PlayOnDamageVFX();
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


 private void TakeKnockback(Transform damageDealer, float physicalDamageTaken)
    {
        float duration = CalcutateKnockbackDuration(physicalDamageTaken);

        Vector2 knockback = CalculateKnockbackDirection (physicalDamageTaken, damageDealer);

        entity?.ReceiveKnockback(knockback, duration);
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