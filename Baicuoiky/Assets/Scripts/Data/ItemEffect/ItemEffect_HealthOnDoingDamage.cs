using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal On Doing Damage", fileName = "Item Effect data- Health On Doing Damage")]

public class ItemEffect_HealthOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float perCentHealedOnAttack = 0.2f; // 20% of damage dealt is healed

    public override void Subcribe(Player player)
    {
        base.Subcribe(player);
        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }
    public override void Unsubcribe()
    {
        base.Unsubcribe();
        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }
  private void HealOnDoingDamage(float damageDealt)
    {
        player.health.IncreaseHealth(damageDealt * perCentHealedOnAttack);
    }
   
}
