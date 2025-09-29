using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        // 1 : Try to find empty slot and equip item
        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
        // 2 : No empty slot found, replace the first one
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;
        UnequipItem(itemToUnequip,slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float saveHealthPercent = player.health.GetHealthPercent();
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);
        slot.equipedItem.AddIitemEffect(player);
        player.health.SetHealthToPercent(saveHealthPercent);
        RemoveItem(itemToEquip);
    }
    public void UnequipItem(Inventory_Item itemToUnequip,bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
        {
            Debug.Log("No space in inventory");
            return;
        }
        float saveHealthPercent = player.health.GetHealthPercent();

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);
        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;
            
        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();
        player.health.SetHealthToPercent(saveHealthPercent);
        AddItem(itemToUnequip);
    }

}