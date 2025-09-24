using UnityEngine;
using System.Collections.Generic;

public class Ui_Inventory : MonoBehaviour
{
    private Ui_ItemSlot[] uiItemSlots;
    private Inventory_Base inventory;
    private void Awake()
    {
        uiItemSlots = GetComponentsInChildren<Ui_ItemSlot>();
        inventory = FindFirstObjectByType<Inventory_Base>();
        inventory.OnInventoryChange += UpdateInventorySlots;
        UpdateInventorySlots();
    }
    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList; // 2 items

        for (int i = 0; i < uiItemSlots.Length; i++) // 10 ui slots
        {
            if (i < itemList.Count)
            {
                uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        }
    }

}
