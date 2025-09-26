using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatsSlot[] uiStatsSlots;
   private Inventory_Player inventory;
    void Awake()
    {
        uiStatsSlots = GetComponentsInChildren<UI_StatsSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateStatsUI;
    }
    private void Start()
    {
        UpdateStatsUI();
    }

       public void UpdateStatsUI()
    {
        foreach (var statsSlot in uiStatsSlots)
        {
            statsSlot.UpdateStatValue();
        }
    }
}
