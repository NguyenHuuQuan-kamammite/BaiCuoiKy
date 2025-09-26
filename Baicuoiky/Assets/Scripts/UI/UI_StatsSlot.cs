using UnityEngine;
using TMPro;
using System.Reflection;
using UnityEngine.EventSystems;

public class UI_StatsSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rect;
    private UI ui;
    private Player_Stats playerStats;
    [SerializeField] private Stats_Type statsSlotType;
    [SerializeField] private TextMeshProUGUI statsName;
    [SerializeField] private TextMeshProUGUI statsValue;
    void OnValidate()
    {
        gameObject.name = "UI_Stats - " + GetStatNameByType(statsSlotType);
        statsName.text = GetStatNameByType(statsSlotType);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
         ui.statsToolTip.ShowToolTip(true, rect, statsSlotType);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statsToolTip.ShowToolTip(false, null, statsSlotType);
    }
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }
    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statsSlotType);

        if (statToUpdate == null && statsSlotType != Stats_Type.ElementalDamage)
            return;

        float value = 0;

        switch (statsSlotType)
        {
            // Major stats
            case Stats_Type.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case Stats_Type.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case Stats_Type.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case Stats_Type.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;
            //Offense stats
            case Stats_Type.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case Stats_Type.CritChance:
                value = playerStats.GetCritChance();
                break;
            case Stats_Type.CritPower:
                value = playerStats.GetCritDamage();
                break;
            case Stats_Type.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100f; // convert to percentage
                break;
            case Stats_Type.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100f; // convert to percentage
                break;

            // Defense stats
            case Stats_Type.Armor:
                value = playerStats.GetBaseArmor();
                break;
            case Stats_Type.Evasion:
                value = playerStats.GetEvasion();
                break;
            case Stats_Type.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case Stats_Type.HealthRegen:
                value = playerStats.resources.healthRegen.GetValue();
                break;

            // Elemental Damage stats
            case Stats_Type.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case Stats_Type.IceDamage:
                value = playerStats.offense.iceDamage.GetValue();
                break;
            case Stats_Type.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            case Stats_Type.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element, 1);
                break;
            // Elemental Resistance stats
            case Stats_Type.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100f; // convert to percentage
                break;
            case Stats_Type.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100f; // convert to percentage
                break;
            case Stats_Type.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100f; // convert to percentage
                break;

        }
        statsValue.text = IsPercentageStat(statsSlotType) ? value + "%" : value.ToString();
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
            case Stats_Type.ElementalDamage: return "Elemental Damage";
            case Stats_Type.Armor: return "Armor";
            case Stats_Type.Evasion: return "Evasion";
            case Stats_Type.IceResistance: return "Ice Resistance";
            case Stats_Type.FireResistance: return "Fire Resistance";
            case Stats_Type.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }
}
// Remove this redundant interface declaration, as IPointerEnterHandler and IPointerExitHandler are provided by UnityEngine.EventSystems.
