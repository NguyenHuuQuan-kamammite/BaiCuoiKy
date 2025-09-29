using UnityEngine;
using System.Collections.Generic;

public class Ui_Inventory : MonoBehaviour
{
 
    private Inventory_Player inventory;
    private UI_EquipSlot[] uiEquipSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private Transform uiEquipSlotParent;
    private void Awake()
    {
       
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
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
    
}
