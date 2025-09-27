using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using Unity.VisualScripting;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;
    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }
    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == Item_Type.Material)

        {
            return "Used for crafting.";
        }
        if (item.itemData.itemType == Item_Type.Consumable)
        {
            return item.itemData.itemEffects.effectDescription;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+" + modValue + " " + modType);
        }
        if (item.itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique Effect:");
            sb.AppendLine(item.itemEffect.effectDescription);
        }
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
