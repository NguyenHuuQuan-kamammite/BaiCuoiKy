using UnityEngine;
using System.Collections.Generic;

public class Inventory_Storage : Inventory_Base
{
    private Inventory_Player playerInventory;
    public List<Inventory_Item> materialStash;

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
