using UnityEngine;
using System;

using System.Text;
using Unity.VisualScripting;
[Serializable]
public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO itemEffect;

public int buyPrice { get; private set; }
public float sellPrice{ get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffects;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * .35f;
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
            statToModify.RemoveModifier(itemId);
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
    
     public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        if (itemData.itemType == Item_Type.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Use for Crafting");
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        if (itemData.itemType == Item_Type.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDescription);
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }


        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+" + modValue + " " + modType);
        }
        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique Effect:");
            sb.AppendLine(itemEffect.effectDescription);
        }
        sb.AppendLine("");
        sb.AppendLine("");
        return sb.ToString();
    }
    private string GetStatNameByType(Stats_Type type)
    {
        switch (type)
        {
            case Stats_Type.MaxHealth: return "Max Health";
            case Stats_Type.HealthRegen: return "Health Regeneration";
            case Stats_Type.Strength: return "Strength";
            case Stats_Type.Agility: return "Agility";
            case Stats_Type.Intelligence: return "Intelligence";
            case Stats_Type.Vitality: return "Vitality";
            case Stats_Type.AttackSpeed: return "Attack Speed";
            case Stats_Type.Damage: return "Damage";
            case Stats_Type.CritChance: return "Critical Chance";
            case Stats_Type.CritPower: return "Critical Power";
            case Stats_Type.ArmorReduction: return "Armor Reduction";
            case Stats_Type.FireDamage: return "Fire Damage";
            case Stats_Type.IceDamage: return "Ice Damage";
            case Stats_Type.LightningDamage: return "Lightning Damage";
            case Stats_Type.Armor: return "Armor";
            case Stats_Type.Evasion: return "Evasion";
            case Stats_Type.IceResistance: return "Ice Resistance";
            case Stats_Type.FireResistance: return "Fire Resistance";
            case Stats_Type.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }
    private bool IsPercentageStat(Stats_Type type)
{
    switch (type)
    {
        case Stats_Type.CritChance:
        case Stats_Type.CritPower:
        case Stats_Type.ArmorReduction:
        case Stats_Type.IceResistance:
        case Stats_Type.FireResistance:
        case Stats_Type.LightningResistance:
        case Stats_Type.AttackSpeed:
        case Stats_Type.Evasion:
            return true;
        default:
            return false;
    }
}
 }
