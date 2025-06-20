using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;
    public Stat vitality;
  

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetVValue();
        float bonusHp = vitality.GetVValue() * 5f; //each point of vitality increases max hp by 5
        
        return baseHp + bonusHp;
    }
}
