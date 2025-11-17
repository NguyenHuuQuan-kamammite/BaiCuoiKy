using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlotParent : MonoBehaviour
{
    private Ui_ItemSlot[] slots;
    public void UpdateSlots(List<Inventory_Item> itemList)
    {
        if (slots == null)
                slots = GetComponentsInChildren<Ui_ItemSlot>();
       for (int i = 0; i < slots.Length; i++) // 10 ui slots
        {
            if (i < itemList.Count)
            {
                slots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                slots[i].UpdateSlot(null);
            }
        }
    }
}
