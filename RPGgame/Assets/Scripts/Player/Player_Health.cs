using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    private Entity_SFX sfx;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        sfx = GetComponent<Entity_SFX>();
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
}
