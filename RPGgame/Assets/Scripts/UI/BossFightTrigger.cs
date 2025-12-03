using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private Enemy_Reaper boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Attach boss health UI now
        UI_BossHealthBar.instance.AttachToBoss(boss.GetComponent<Enemy_Health>());

        // You can also make the boss active, or play cutscene, etc.
        Debug.Log("Boss Fight Started!");

        // Disable the trigger so it activates only once
        gameObject.SetActive(false);
    }
}