using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Ui_Inventory : MonoBehaviour
{
 
    private Inventory_Player inventory;
    
    [SerializeField] private UI_ItemSlotParent inventorySlotParent;
    [SerializeField] private UI_EquipSlotParent equipSlotParent;
    [SerializeField] private TextMeshProUGUI goldText;
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
        goldText.text = inventory.gold.ToString("N0") + "g.";
    }
   
    
}
