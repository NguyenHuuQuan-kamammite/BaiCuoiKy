using UnityEngine;
using UnityEngine.UI;
using System;

public class Entity_Health : MonoBehaviour, IDamgable
{
    public event Action OnTakingDamage;
    public event Action OnHealthUpdate;

    private Slider healthBar;
    private Entity_VFX entityVFX;

    private Entity entity;
    private bool miniHealthBarActice;
    private Entity_Stats stats;
    private Entity_DropManager dropManager;

    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegenerateHealth = true;
    [SerializeField] protected float currentHp;
    public bool isDead { get; private set; }
    public float lastDamegeTaken { get; private set; }
    protected bool canTakeDamage = true;

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
        dropManager = GetComponent<Entity_DropManager>();
    }


    protected virtual void Start()
    {

        SetUpHealth();
    }

    private void SetUpHealth()
    {
        if (stats == null)
            return;
        currentHp = stats.GetMaxHealth();
        OnHealthUpdate += UpdateHealthBar;
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false)
            return false;
        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0f;

        float mitigation = stats != null ? stats.GetArmorMitigation(armorReduction) : 0f;
        float resistance = stats != null ? stats.GetElementalResistance(element) : 0f;

        float physicalDamageTaken = damage * (1 - mitigation);

        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);

        ReduceHp(physicalDamageTaken + elementalDamageTaken);

        lastDamegeTaken = physicalDamageTaken + elementalDamageTaken;

        OnTakingDamage?.Invoke();

        return true;
    }
    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;
    public void IncreaseHealth(float healthAmount)
    {
        if (isDead) return;
        float newHealth = currentHp + healthAmount;
        float maxHealth = stats.GetMaxHealth();
        currentHp = Mathf.Min(newHealth, maxHealth);

        OnHealthUpdate?.Invoke();
        
    }
    private void RegenerateHealth()
    {
        if (isDead || !canRegenerateHealth) return;

        float regenAmount = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    private bool AttackEvaded()
    {
        if (stats == null)
            return false;
        else
        {
            return UnityEngine.Random.Range(0f, 100f) < stats.GetEvasion();
        }
    }
    public void ReduceHp(float damage)
    {
        currentHp -= damage;
        entityVFX?.PlayOnDamageVFX();
        OnHealthUpdate?.Invoke();
        if (currentHp <= 0)
        {
            
            Die();
        }

    }

    protected virtual void Die()
    {
        isDead = true;

        entity.EntityDeath();
        dropManager.DropItems();
    }

    public float GetHealthPercent()
    {
        return currentHp / stats.GetMaxHealth();
    }

    public void SetHealthToPercent(float percent)
    {
        if (isDead) return;

        currentHp = Mathf.Clamp(percent, 0, 1) * stats.GetMaxHealth();
        OnHealthUpdate?.Invoke();

    }
    public float GetCurrentHealth() => currentHp;
    private void UpdateHealthBar()
    {
        if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
            return;
        healthBar.value = currentHp / stats.GetMaxHealth();
    }
    public void EnableHealthBar(bool enable) => healthBar?.transform.parent.gameObject.SetActive(enable);

    private void TakeKnockback(Transform damageDealer, float physicalDamageTaken)
    {
        float duration = CalcutateKnockbackDuration(physicalDamageTaken);

        Vector2 knockback = CalculateKnockbackDirection(physicalDamageTaken, damageDealer);

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
        if (stats == null)
            return false;

        else

            return damage / stats.GetMaxHealth() > heavyKnockbackThreshold;
    }
}