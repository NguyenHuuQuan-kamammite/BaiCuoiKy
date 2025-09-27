using UnityEngine;
using System;

[Serializable]
public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO itemEffect;
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffects;
        itemId = itemData.itemName + " - " + Guid.NewGuid();

    }
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }
    public void RemoveModifiers(Entity_Stats playerStats)
    {
         foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier( itemId);
        }
    }
    public void AddIitemEffect(Player player)
    {
        itemEffect?.Subcribe(player);
    }
    public void RemoveItemEffect()
    {
        itemEffect?.Unsubcribe();
    }
    private EquipmentData_SO EquipmentData()
    {
        if (itemData is EquipmentData_SO equipment)
            return equipment;
        return null;
    }
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
 }
