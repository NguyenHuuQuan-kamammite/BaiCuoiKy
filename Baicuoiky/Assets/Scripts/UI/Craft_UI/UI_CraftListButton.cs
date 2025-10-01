using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData;
    private UI_CraftSlot[] craftSlots;

    public void SetCraftSlot(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlot()
    {
        if (craftData == null)
        {
            Debug.LogWarning("Craft list is null!");
            return;
        }
        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }
        for (int i = 0; i < craftData.itemList.Length ; i++)
        {
            ItemDataSO itemData = craftData.itemList[i];
            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetUpButton(itemData);
        }
    }
}
