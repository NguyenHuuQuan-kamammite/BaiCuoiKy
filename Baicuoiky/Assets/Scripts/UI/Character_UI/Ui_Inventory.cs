using UnityEngine;
using System.Collections.Generic;

public class Ui_Inventory : MonoBehaviour
{
 
    private Inventory_Player inventory;
    
    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_EquipSlotParent equipSlotParent;
    private void Awake()
    {
       
       

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    private void UpdateUI()
    {
        inventorySlotParent.UpdateSlots(inventory.itemList);
       equipSlotParent.UpdateEquipmentSlot(inventory.equipList );
    }
   
    
}
