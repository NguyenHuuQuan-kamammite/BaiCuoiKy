using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_BossHealthBar : MonoBehaviour
{
    public static UI_BossHealthBar instance;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private Enemy_Health bossHealth;

    private void Awake()
    {
        instance = this;
        Hide(); // hide until boss fight begins
    }

    public void AttachToBoss(Enemy_Health health)
    {
        bossHealth = health;

        // Subscribe to health update event
        bossHealth.OnHealthUpdate += UpdateBossUI;

        // Initialize UI one time
        UpdateBossUI();
        Show();
    }

    private void UpdateBossUI()
    {
        if (bossHealth == null) return;

        float current = Mathf.RoundToInt(bossHealth.GetCurrentHealth());
        float max = bossHealth.GetMaxHealth();

        // Resize bar automatically to match max HP
        healthRect.sizeDelta = new Vector2(max, healthRect.sizeDelta.y);

        healthText.text = current + "/" + max;
        healthSlider.value = bossHealth.GetHealthPercent();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}

