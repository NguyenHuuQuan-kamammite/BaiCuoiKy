using TMPro;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{

    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private TextMeshProUGUI goldText;
    [Space]
    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorSlots;
    [SerializeField] private UI_EquipSlotParent equipSlots;
   public void SetUpMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        this.inventory.OnInventoryChange += UpdatesSlotUI;
        this.merchant.OnInventoryChange += UpdatesSlotUI;
        UpdatesSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchantSlots)
        {
            slot.SetupMerchantUI(merchant);
        }
       
    }
    private void UpdatesSlotUI()
    {

        if (inventory == null)
            return;
        merchantSlots.UpdateSlots(merchant.itemList);
        inventorSlots.UpdateSlots(inventory.itemList);
        equipSlots.UpdateEquipmentSlot(inventory.equipList);  
        goldText.text = inventory.gold.ToString("N0") + "g.";
    }
}
