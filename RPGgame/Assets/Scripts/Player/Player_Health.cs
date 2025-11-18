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

    protected override void Die()
    {
        base.Die();
        sfx?.PlayDead();
        player.ui.OpenDeathScreenUI();
        //GameManager.instance.SetLastPlayerPosition(transform.position);
        //GameManager.instance.RestartScene();

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
            data.playerCurrentHealth = Mathf.RoundToInt(currentHp);
            data.playerMaxHealth = Mathf.RoundToInt(playerStats.GetMaxHealth());

            
        }
    }
}
