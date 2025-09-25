using UnityEngine;
using System.Collections.Generic;

public class Ui_Inventory : MonoBehaviour
{
    private Ui_ItemSlot[] uiItemSlots;
    private Inventory_Player inventory;
    private UI_EquipSlot[] uiEquipSlots;
    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;
    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<Ui_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipSlots();
    }
    private void UpdateEquipSlots()
    {
        List<Inventory_EquipmentSlot> PlayerEquipList = inventory.equipList; // 2 slots

        for (int i = 0; i < uiEquipSlots.Length; i++) // 10 ui slots
        {
            var playerEquipSlot = PlayerEquipList[i];
            if (playerEquipSlot.HasItem() == false)
            {
                uiEquipSlots[i].UpdateSlot(null);

            }
            else
            {
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
            }
        }
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
