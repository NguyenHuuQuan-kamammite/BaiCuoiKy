using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Refund All Skills", fileName = "Item Effect data- Refund All Skills")]
public class ItemEffect_RefundAllSkill : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
       UI ui = FindFirstObjectByType<UI>();
      
        ui.skillTree.RefundAllSkillPoints();
    }
}
