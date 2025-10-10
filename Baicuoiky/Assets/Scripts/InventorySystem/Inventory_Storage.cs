using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory{ get; private set; }
    public List<Inventory_Item> materialStash;


    public void CraftItem( Inventory_Item itemToCraft)
    {
        ConsumeMaterial(itemToCraft);
        playerInventory.AddItem(itemToCraft);
    }
    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterial(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }
    private void ConsumeMaterial(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requiredItem.stackSize; ;
            amountToConsume = amountToConsume - ConsumedMaterialAmount(playerInventory.itemList, requiredItem);

            if (amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialAmount(itemList, requiredItem);
            if (amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialAmount(materialStash, requiredItem);
        }
    }

    private int ConsumedMaterialAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {

        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;

        // Iterate over a copy to avoid modifying the collection during enumeration
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            var item = itemList[i];
            if (item.itemData != neededItem.itemData)
            {
                continue;
            }
            int removeAmount = Mathf.Min(item.stackSize, amountNeeded - consumedAmount);
            item.stackSize = item.stackSize - removeAmount;
            consumedAmount = consumedAmount + removeAmount;
            if (item.stackSize <= 0)
                itemList.RemoveAt(i);
            if (consumedAmount >= amountNeeded)
                break;
        }
        return consumedAmount;

    }
    private bool HasEnoughMaterial(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }
        return true;
    }
    public int GetAvailableAmountOf(ItemDataSO requireItem)
    {
        int amount = 0;
        foreach (var item in playerInventory.itemList)
        {
            if (item.itemData == requireItem)
                amount = amount + item.stackSize;
        }
        foreach (var item in itemList)
        {
            if (item.itemData == requireItem)
                amount = amount + item.stackSize;
        }
        foreach (var item in materialStash)
        {
            if (item.itemData == requireItem)
                amount = amount + item.stackSize;
        }
        return amount;
    }
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableStash(itemToAdd);
        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(newItemToAdd);
        }

        TriggerUpdateUI();
        materialStash =materialStash.OrderBy(item => item.itemData.name).ToList();
    }
    public void RemoveMaterialFromStash(Inventory_Item item)
    {
        materialStash.Remove(item);
    }
 public Inventory_Item StackableStash(Inventory_Item itemToAdd)
    {
      return materialStash.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());
  
    }

    public void SetInventory(Inventory_Player inventory)
    {
        this.playerInventory = inventory;
    }
    public void FromPlayerToStorage(Inventory_Item item, bool tranferFullStack)
    {
        int transferAmount = tranferFullStack ? item.stackSize : 1;
        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItem(item))
                {
                    var itemToTAdd = new Inventory_Item(item.itemData);
                    playerInventory.RemoveOneItem(item);
                    AddItem(itemToTAdd);
                }
 
        }
        TriggerUpdateUI();
       
    }
    public void FromStorageToPlayer(Inventory_Item item, bool tranferFullStack)
    {
        int transferAmount = tranferFullStack ? item.stackSize : 1;
        for (int i = 0; i < transferAmount; i++)
        {

            if (playerInventory.CanAddItem(item))
                {
                    var itemToTAdd = new Inventory_Item(item.itemData);
                    RemoveOneItem(item);
                    playerInventory.AddItem(itemToTAdd);
                }
        }
        TriggerUpdateUI();
     
    }
    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);
        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {

                string saveId = item.itemData.saveID;


                if (data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;
                data.storageItems[saveId] += item.stackSize;
            }
        }
        data.storageMaterials.Clear();

        foreach (var item in materialStash)
        {
            if (item != null && item.itemData != null)
            {

                string saveId = item.itemData.saveID;


                if (data.storageMaterials.ContainsKey(saveId) == false)
                    data.storageMaterials[saveId] = 0;
                data.storageMaterials[saveId] += item.stackSize;
            }
        }
    }
    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }

            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }

        }
        foreach (var entry in data.storageMaterials)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddMaterialToStash(itemToLoad);
            }
        }
    }
}
