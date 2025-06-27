using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;

    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    public float GetPhysicalDamage(out bool isCritical)
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

        isCritical =  UnityEngine.Random.Range(0f, 100f) < critChance;
        float finalDamage = isCritical ? totalDamage * critPower : totalDamage;

        return finalDamage;
    }


    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
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
    public float GetArmorMitigation(float armorReduction )
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.strength.GetValue(); // each point of strength increases armor by 1
        float totalArmor = baseArmor + bonusArmor;

        float reductionMultiplier = Math.Clamp(1 - armorReduction,0,1 ); // convert armor reduction percentage to a multiplier
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
}
