using UnityEngine;
using System.Collections.Generic;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory{ get; private set; }
    public List<Inventory_Item> materialStash;

    public void ConsumeMaterial(Inventory_Item itemToCraft)
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
    public bool HasEnoughMaterial(Inventory_Item itemToCraft)
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
            materialStash.Add(itemToAdd);

        TriggerUpdateUI();
    }
    public void RemoveMaterialFromStash(Inventory_Item item)
    {
        materialStash.Remove(item);
    }
 public Inventory_Item StackableStash(Inventory_Item itemToAdd)
    {
      List<Inventory_Item> stashableItems = materialStash.FindAll(item => item.itemData == itemToAdd.itemData);
        foreach(var stackable in stashableItems)
        {
            if (stackable.CanAddStack())
            {
               
                return stackable;
            }
        }
        return null;
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
}
