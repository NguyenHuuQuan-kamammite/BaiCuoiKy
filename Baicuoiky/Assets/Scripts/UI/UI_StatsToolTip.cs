using UnityEditor;
using UnityEngine;
using TMPro;

public class UI_StatsToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;
    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }
     public void ShowToolTip(bool show, RectTransform targetRect,Stats_Type statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(Stats_Type type)
    {
        switch (type)
        {
            // Major Attributes
            case Stats_Type.Strength:
                return "Increases physical damage by 1 per point." +
                       "\n Increases critical power by 0.5% per point.";
            case Stats_Type.Agility:
                return "Increases critical chance by 0.3% per point." +
                       "\n Increases evasion by 0.5% per point.";
            case Stats_Type.Intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                        "\n Adds 1 elemental damage per point as a bonus. " +
                        "\n If all elements have 0 damage, the bonus will not be applied.";
            case Stats_Type.Vitality:
                return "Increases maximum health by 5 per point" +
                       "\n Increases armor by 1 per point.";

            // Physical Damage
            case Stats_Type.Damage:
                return "Determines the physical damage of your attacks.";
            case Stats_Type.CritChance:
                return "Chance for your attacks to critically strike.";
            case Stats_Type.CritPower:
                return "Increases the damage dealt by critical strikes.";
            case Stats_Type.ArmorReduction:
                return "Percent of armor that will be ignored by your attacks.";
            case Stats_Type.AttackSpeed:
                return "Determines how quickly you can attack.";

            // Defense
            case Stats_Type.MaxHealth:
                return "Determines how much total health you have.";
            case Stats_Type.HealthRegen:
                return "Amount of health restored per second.";
            case Stats_Type.Armor:
                return "Reduces incoming physical damage."
                    + "\n Armor mitigation is Limited at 85%."
                    + "Current mitigation is: " + playerStats.GetArmorMitigation(0) * 100 + "%.";
            case Stats_Type.Evasion:
                return "Chance to completely avoid attacks." + "\n Limited at 85%.";

            // Elemental Damage
            case Stats_Type.IceDamage:
                return "Determines the ice damage of your attacks.";
            case Stats_Type.FireDamage:
                return "Determines the fire damage of your attacks.";
            case Stats_Type.LightningDamage:
                return "Determines the lightning damage of your attacks.";
            case Stats_Type.ElementalDamage:
                return
                    "Elemental damage combines all three elements. " +
                    "\n The highest element applies corresponding element status effect and full damage. " +
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            // Elemental Resistances
            case Stats_Type.IceResistance:
                return "Reduces ice damage taken.";
            case Stats_Type.FireResistance:
                return "Reduces fire damage taken.";
            case Stats_Type.LightningResistance:
                return "Reduces lightning damage taken.";

            default:
                return "No tooltip avalible for this stat.";
        }
    }
}
