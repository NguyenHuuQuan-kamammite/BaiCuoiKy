using UnityEngine;

public class Spike_InstantKill : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private bool instantKill = true; // If false, deals damage instead
    [SerializeField] private float damageAmount = 9999f; // Damage if not instant kill

    [Header("Knockback (if not instant kill)")]
    [SerializeField] private bool applyKnockback = false;
    [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 5f);
    [SerializeField] private float knockbackDuration = 0.3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it's the player by layer
        if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0)
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if it's the player by layer
        if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0)
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void KillPlayer(GameObject playerObject)
    {
        Player_Health playerHealth = playerObject.GetComponent<Player_Health>();

        if (playerHealth == null)
        {
            Debug.LogWarning("Spike hit something on Player layer but couldn't find Player_Health component!");
            return;
        }

        if (playerHealth.isDead)
        {
            return; // Player is already dead, don't kill again
        }

        if (instantKill)
        {
            // Method 1: Direct instant kill by reducing HP to 0
            playerHealth.ReduceHp(playerHealth.GetCurrentHealth());
        }
        else
        {
            // Method 2: Deal massive damage (respects armor/resistance)
            Entity entity = playerObject.GetComponent<Entity>();

            if (applyKnockback && entity != null)
            {
                // Calculate knockback direction (away from spike)
                int direction = playerObject.transform.position.x > transform.position.x ? 1 : -1;
                Vector2 knockback = new Vector2(knockbackForce.x * direction, knockbackForce.y);
                entity.ReceiveKnockback(knockback, knockbackDuration);
            }

            // Deal damage through the proper damage system
            playerHealth.TakeDamage(damageAmount, 0, ElementType.None, transform);
        }
    }

    // Optional: Visual feedback in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
