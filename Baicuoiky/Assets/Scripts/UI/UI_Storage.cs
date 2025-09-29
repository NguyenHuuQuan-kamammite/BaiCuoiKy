using Unity.VisualScripting;
using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Storage storage;
    private Inventory_Player inventory;
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;
    public void SetUpStorage(Inventory_Player inventory, Inventory_Storage storage)
    {
        this.storage = storage;
        this.inventory = inventory;
        storage.OnInventoryChange += UpdateUI;
     
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();
        foreach (var slot in storageSlots)
            slot.SetStorage(storage);
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
    }
}
