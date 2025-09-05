using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;
    public Stat_Resources resources;

    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_MajorGroup major;


    public AttackData GetAttackData(DamageScaleData scaleData)
        {
            return new AttackData(this, scaleData);
        }

    public float GetElementalDamage(out ElementType element, float scaleFactor)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float bonusElementalDamage = major.intelligence.GetValue(); // each point of intelligence increases elemental damage by 1

        if (fireDamage <= 0 && iceDamage <= 0 && lightningDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float highestElementalDamage = fireDamage;
        element = ElementType.Fire;
        if (iceDamage > highestElementalDamage)
        {
            highestElementalDamage = iceDamage;
            element = ElementType.Ice;
        }
        if (lightningDamage > highestElementalDamage)
        {
            highestElementalDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        float bonusFire = (fireDamage == highestElementalDamage) ? 0 : fireDamage * 0.5f; // if fire damage is not the highest, give it a bonus
        float bonusIce = (iceDamage == highestElementalDamage) ? 0 : iceDamage * 0.5f; // if ice damage is not the highest, give it a bonus
        float bonusLightning = (lightningDamage == highestElementalDamage) ? 0 : lightningDamage * 0.5f; // if lightning damage is not the highest, give it a bonus

        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning; // sum of weaker elemental damages
        float finalDamage = highestElementalDamage + weakerElementalDamage + bonusElementalDamage;
        return finalDamage * scaleFactor;
    }
    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0f;
        float bonusResistance = major.intelligence.GetValue() * 0.5f;

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
            default:
                return 0f; // no resistance for other elements
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 0.75f;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap); // cap resistance at 75%
        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCritical, float scaleFactor = 1f)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue(); // each point of strength increases physical damage by 1
        float totalDamage = baseDamage + bonusDamage;


        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f; // each point of agility increases crit chance by 0.3%
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100f; // each point of strength increases crit power by 0.5%

        isCritical = UnityEngine.Random.Range(0f, 100f) < critChance;
        float finalDamage = isCritical ? totalDamage * critPower : totalDamage;

        return finalDamage * scaleFactor;
    }


    public float GetMaxHealth()
    {
        float baseHp = resources.maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5f; //each point of vitality increases max hp by 5
        float totalHp = baseHp + bonusHp;
        return totalHp;
    }
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f; // each point of agility increases evasion by 0.5%
        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 85f; // cap evasion at 85%
        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);
        return finalEvasion;
    }
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.strength.GetValue(); // each point of strength increases armor by 1
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Math.Clamp(1 - armorReduction, 0, 1); // convert armor reduction percentage to a multiplier
        float effectiveArmor = totalArmor * reductionMultiplier; // apply armor reduction to total armor

        float mitigration = effectiveArmor / (effectiveArmor + 100f); // armor mitigation formula
        float mitigrationCap = 0.85f; // cap armor mitigation at 85%

        float finalMitigation = Mathf.Clamp(mitigration, 0, mitigrationCap);

        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100f; // armor reduction is a percentage
        return finalReduction;
    }
    public Stat GetStatByType(Stats_Type type)

    {
        switch (type)
        {
            case Stats_Type.MaxHealth:
                return resources.maxHealth;
            case Stats_Type.HealthRegen:
                return resources.healthRegen;
            case Stats_Type.Strength:
                return major.strength;
            case Stats_Type.Agility:
                return major.agility;
            case Stats_Type.Intelligence:
                return major.intelligence;
            case Stats_Type.Vitality:
                return major.vitality;
            case Stats_Type.AttackSpeed:
                return offense.attackSpeed;
            case Stats_Type.Damage:
                return offense.damage;
            case Stats_Type.CritChance:
                return offense.critChance;
            case Stats_Type.CritPower:
                return offense.critPower;
            case Stats_Type.ArmorReduction:
                return offense.armorReduction;
            case Stats_Type.FireDamage:
                return offense.fireDamage;
            case Stats_Type.IceDamage:
                return offense.iceDamage;
            case Stats_Type.LightningDamage:
                return offense.lightningDamage;
            case Stats_Type.Armor:
                return defense.armor;
            case Stats_Type.Evasion:
                return defense.evasion;
            case Stats_Type.IceResistance:
                return defense.iceRes;
            case Stats_Type.FireResistance:
                return defense.fireRes;
            case Stats_Type.LightningResistance:
                return defense.lightningRes;
            default:
                Debug.LogWarning($"Stat of type {type} not found.");
                return null;
        }
    }


[ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.LogWarning("Default Stat Setup is not assigned.");
            return;
        }
        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningResistance);
    }
}
