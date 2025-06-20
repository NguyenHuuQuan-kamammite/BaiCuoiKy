using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;

    public Stat_MajorGroup major;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
 
  

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5f; //each point of vitality increases max hp by 5

        return baseHp + bonusHp;
    }
}
