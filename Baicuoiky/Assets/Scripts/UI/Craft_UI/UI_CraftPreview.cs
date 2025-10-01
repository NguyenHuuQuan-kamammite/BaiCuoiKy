using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewMaterialSlot[] craftPreviewSlots;

    [Header("Item Preview Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;





    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewMaterialSlot>();
        foreach (var slot in craftPreviewSlots)
            slot.gameObject.SetActive(false);

    }

    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            buttonText.text = " Pick an item.";
            return;
        }
        if (storage.HasEnoughMaterial(itemToCraft) && storage.playerInventory.CanAddItem(itemToCraft))
        {
            storage.ConsumeMaterial(itemToCraft);
            storage.playerInventory.AddItem(itemToCraft);
        }
        UppdateCraftPreviewSlots();
    }
    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();

        UppdateCraftPreviewSlots();
    }
    private void UppdateCraftPreviewSlots()
    {
        foreach (var slot in craftPreviewSlots)
            slot.gameObject.SetActive(false);
        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requireAmount = requiredItem.stackSize;

            craftPreviewSlots[i].gameObject.SetActive(true);
            craftPreviewSlots[i].SetUpMaterialSlot(requiredItem.itemData, availableAmount, requireAmount);
        }
    }
}
