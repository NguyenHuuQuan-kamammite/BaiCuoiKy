using UnityEngine;

public class UI_Merchant : MonoBehaviour
{

    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorSlots;
   public void SetUpMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        merchant.OnInventoryChange += UpdatesSlotUI;
        

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchantSlots)
        {
            slot.SetupMerchantUI(merchant);
        }
        UpdatesSlotUI();
    }
    private void UpdatesSlotUI()
    {
        inventorSlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
    }
}
