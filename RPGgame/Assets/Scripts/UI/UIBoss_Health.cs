using UnityEngine;

public class UIBoss_Health : Entity_Health
{
    [Header("Boss Settings")]
    [SerializeField] private string bossName = "Reapper";
    [SerializeField] private float combatTimeout = 5f;

    private Boss_HealthBar bossHealthBar;
    private float lastCombatTime;
    private bool isInCombat;

    protected override void Start()
    {
        base.Start();

        // Hide mini health bar on boss
        if (healthBar != null)
            healthBar.transform.parent.gameObject.SetActive(false);

        // Find the UI boss health bar
        bossHealthBar = FindFirstObjectByType<Boss_HealthBar>();
        if (bossHealthBar != null)
        {
            bossHealthBar.SetupBossHealthBar(this, bossName);
        }
    }

    private void Update()
    {
        if (isInCombat && Time.time - lastCombatTime > combatTimeout)
        {
            ExitCombat();
        }
    }

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        bool tookDamage = base.TakeDamage(damage, elementalDamage, element, damageDealer);

        if (tookDamage)
        {
            EnterCombat();
        }

        return tookDamage;
    }

    private void EnterCombat()
    {
        if (!isInCombat)
        {
            isInCombat = true;
            if (bossHealthBar != null)
                bossHealthBar.ShowHealthBar();
        }

        lastCombatTime = Time.time;
    }

    private void ExitCombat()
    {
        isInCombat = false;
        if (bossHealthBar != null)
            bossHealthBar.HideHealthBar();
    }

    protected override void UpdateHealthBar()
    {
        // Boss uses the UI health bar via OnHealthUpdate event
    }

    protected override void Die()
    {
        if (bossHealthBar != null)
            bossHealthBar.HideHealthBar();

        base.Die();
    }
}
