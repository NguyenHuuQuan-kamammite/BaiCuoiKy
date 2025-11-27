using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player_Health : Entity_Health, ISaveable
{
    private Player player;

    private Entity_SFX sfx;
    private bool healthRestored = false;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        sfx = GetComponent<Entity_SFX>();
        enabled = false;
    }
    protected override void Start()
    {
      
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Die();
    }
    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        // Prevent taking damage if already dead
        if (currentHp <= 0)
            return false;

        bool tookDamage = base.TakeDamage(damage, elementalDamage, element, damageDealer);

        // Clamp health to prevent going below 0
        if (currentHp < 0)
            currentHp = 0;

        return tookDamage;
    }
    protected override void Die()
    {
        base.Die();
        UpdateHealthBar();
        sfx?.PlayDead();
        RestoreHealthAfterDeath();
        player.ui.OpenDeathScreenUI();
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();

    }
    private void RestoreHealthAfterDeath()
    {
        if (stats != null)
        {
            float maxHealth = stats.GetMaxHealth();
            currentHp = maxHealth * 0.5f; // Restore 50% of max health
            UpdateHealthBar();
            Debug.Log($"Player respawned with {currentHp}/{maxHealth} HP (50% restored)");
        }
    }
    public void LoadData(GameData data)
    {
        // Make sure stats are available
        if (stats == null)
            stats = GetComponent<Entity_Stats>();

        // Initialize the health system with saved data
        if (data.playerMaxHealth > 0)
        {
            // Set currentHp directly first to prevent SetUpHealth from overriding it
            currentHp = data.playerCurrentHealth;

            // Clamp to prevent negative health on load
            if (currentHp < 0)
                currentHp = 0;

            // Now call the setup but prevent it from resetting health
            ForceHealthSetup(data.playerMaxHealth);
        }
        else
        {
            // Fallback to normal setup if no saved data
            base.Start();
        }

        // Re-enable the component
        enabled = true;
    }

    private void ForceHealthSetup(float maxHealth)
    {
        // Set up events and health bar without resetting currentHp
        OnHealthUpdate += UpdateHealthBar;
        UpdateHealthBar();

        if (canRegenerateHealth)
        {
            InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
        }
    }

    public void SaveData(ref GameData data)
    {
        Entity_Stats playerStats = GetComponent<Entity_Stats>();
        if (playerStats != null)
        {
            // Clamp health before saving to prevent saving negative values
            float healthToSave = Mathf.Max(0, currentHp);

            data.playerCurrentHealth = Mathf.RoundToInt(healthToSave);
            data.playerMaxHealth = Mathf.RoundToInt(playerStats.GetMaxHealth());
        }
    }
}
